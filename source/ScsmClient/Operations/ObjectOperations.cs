using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Newtonsoft.Json.Linq;
using Reflectensions;
using Reflectensions.ExtensionMethods;
using ScsmClient.Attributes;
using ScsmClient.ExtensionMethods;
using ScsmClient.Helper;
using ScsmClient.Model;
using ScsmClient.SharedModels.Models;
using IEnumerable = System.Collections.IEnumerable;

namespace ScsmClient.Operations
{
    public class ObjectOperations : BaseOperation
    {

        private ConcurrentDictionary<Guid, Dictionary<string, ManagementPackProperty>> _objectPropertyDictionary = new ConcurrentDictionary<Guid, Dictionary<string, ManagementPackProperty>>();

        public ObjectOperations(SCSMClient client) : base(client)
        {
        }

        public EnterpriseManagementObject GetEnterpriseManagementObjectById(Guid id)
        {
            var critOptions = new ObjectQueryOptions();
            critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.NonBuffered;

            return _client.ManagementGroup.EntityObjects.GetObject<EnterpriseManagementObject>(id, critOptions);
        }

        public IEnumerable<EnterpriseManagementObject> GetEnterpriseManagementObjectsByClassName(string className, string criteria, int? maxResult = null)
        {
            var objectClass = _client.Types().GetClassByName(className);
            return GetEnterpriseManagementObjectsByClass(objectClass, criteria, maxResult);
        }

        public IEnumerable<EnterpriseManagementObject> GetEnterpriseManagementObjectsByClassId(Guid classId, string criteria, int? maxResult = null)
        {
            var objectClass = _client.Types().GetClassById(classId);
            return GetEnterpriseManagementObjectsByClass(objectClass, criteria, maxResult);
        }

        public IEnumerable<EnterpriseManagementObject> GetEnterpriseManagementObjectsByClass(ManagementPackClass objectClass, string criteria, int? maxResult = null)
        {


            var crit = _client.Criteria().BuildObjectCriteria(criteria, objectClass);


            var critOptions = new ObjectQueryOptions();

            critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            if (maxResult.HasValue && maxResult.Value != int.MaxValue)
            {
                critOptions.MaxResultCount = maxResult.Value;
                critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.Buffered;
            }

            var sortprop = new EnterpriseManagementObjectGenericProperty(EnterpriseManagementObjectGenericPropertyName.TimeAdded);
            critOptions.AddSortProperty(sortprop, SortingOrder.Ascending);


            var reader = _client.ManagementGroup.EntityObjects.GetObjectReader<EnterpriseManagementObject>(crit, critOptions);


            foreach (EnterpriseManagementObject enterpriseManagementObject in reader)
            {
                //if (count == critOptions.MaxResultCount)
                //    break;
                yield return enterpriseManagementObject;
            }

        }


        public Guid CreateObjectByClassId(Guid id, Dictionary<string, object> properties)
        {
            var objectClass = _client.Types().GetClassById(id);
            return CreateObjectByClass(objectClass, properties);
        }

        public Guid CreateObjectByClassName(string className, Dictionary<string, object> properties)
        {
            var objectClass = _client.Types().GetClassByName(className);
            return CreateObjectByClass(objectClass, properties);
        }

        public Guid CreateObjectByClass(ManagementPackClass objectClass, Dictionary<string, object> properties)
        {
            return CreateObjectsByClass(objectClass, new[] { properties }, CancellationToken.None).FirstOrDefault().Value;
        }


        public Dictionary<int, Guid> CreateObjectsByClassId(Guid id, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var objectClass = _client.Types().GetClassById(id);
            return CreateObjectsByClass(objectClass, objects, cancellationToken);
        }

        public Dictionary<int, Guid> CreateObjectsByClassName(string className, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var objectClass = _client.Types().GetClassByName(className);
            return CreateObjectsByClass(objectClass, objects, cancellationToken);
        }

        public Dictionary<int, Guid> CreateObjectsByClass(ManagementPackClass objectClass, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var result = new Dictionary<int, Guid>();
            var groups = GroupIn10(objects);

            var index = 0;
            foreach (var enumerable in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var _list = new List<Guid>();
                var idd = new IncrementalDiscoveryData();
                foreach (var dictionary in enumerable)
                {

                    var obj = BuildEnterpriseManagementObjectWithRelations(objectClass, dictionary);
                    var rootId = AddIncremental(obj, ref idd);

                    _list.Add(rootId);
                }
                idd.Commit(_client.ManagementGroup);
                foreach (var guid in _list)
                {
                    result.Add(index++, guid);
                }

            }

            return result;

        }


        public Guid CreateObjectFromTemplateId(Guid id, Dictionary<string, object> properties)
        {

            var template = _client.Template().GetObjectTemplateById(id);
            return CreateObjectFromTemplate(template, properties);
        }

        public Guid CreateObjectFromTemplateName(string templateName, Dictionary<string, object> properties)
        {

            var template = _client.Template().GetObjectTemplateByName(templateName);
            return CreateObjectFromTemplate(template, properties);
        }

        public Guid CreateObjectFromTemplate(ManagementPackObjectTemplate template, Dictionary<string, object> properties)
        {

            return CreateObjectsFromTemplate(template, new[] { properties }, CancellationToken.None).First().Value;
        }

        public Dictionary<int, Guid> CreateObjectsFromTemplateId(Guid id, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var template = _client.Template().GetObjectTemplateById(id);
            return CreateObjectsFromTemplate(template, objects, cancellationToken);
        }

        public Dictionary<int, Guid> CreateObjectsFromTemplateName(string templateName, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var template = _client.Template().GetObjectTemplateByName(templateName);
            return CreateObjectsFromTemplate(template, objects, cancellationToken);
        }

        public Dictionary<int, Guid> CreateObjectsFromTemplate(ManagementPackObjectTemplate template, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var result = new Dictionary<int, Guid>();
            var groups = GroupIn10(objects);
            var index = 0;
            foreach (var enumerable in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var _list = new List<Guid>();
                var idd = new IncrementalDiscoveryData();

                foreach (var dictionary in enumerable)
                {

                    var obj = BuildEnterpriseManagementObjectProjectionWithRelations(template, dictionary);
                    var rootId = AddIncremental(obj, ref idd);

                    _list.Add(rootId);
                }
                idd.Commit(_client.ManagementGroup);
                foreach (var guid in _list)
                {
                    result.Add(index++, guid);
                }
            }

            return result;

        }


        public int DeleteObjectsByClassName(string className, string criteria, CancellationToken cancellationToken = default)
        {
            var obj = GetEnterpriseManagementObjectsByClassName(className, criteria);
            return DeleteObjects(obj, cancellationToken);
        }
        public int DeleteObjectsByClassId(Guid classId, string criteria, CancellationToken cancellationToken = default)
        {
            var obj = GetEnterpriseManagementObjectsByClassId(classId, criteria);
            return DeleteObjects(obj, cancellationToken);
        }
        public int DeleteObjectsByClass(ManagementPackClass objectClass, string criteria, CancellationToken cancellationToken = default)
        {
            var obj = GetEnterpriseManagementObjectsByClass(objectClass, criteria);
            return DeleteObjects(obj, cancellationToken);
        }
        public int DeleteObjects(IEnumerable<EnterpriseManagementObject> objects, CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<int, Guid>();
            var groups = GroupIn10(objects);

            var count = 0;
            foreach (var enumerable in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var idd = new IncrementalDiscoveryData();
                var enterpriseManagementObjects = enumerable as EnterpriseManagementObject[] ?? enumerable.ToArray();
                foreach (var obj in enterpriseManagementObjects)
                {
                    idd.Remove(obj);
                }
                idd.Commit(_client.ManagementGroup);
                count = count + enterpriseManagementObjects.Count();
            }

            return count;
        }


        public void UpdateObject(Guid id, Dictionary<string, object> properties)
        {
            var enterpriseManagementObject = GetEnterpriseManagementObjectById(id);
            UpdateObject(new[] { enterpriseManagementObject }, properties);
        }

        public void UpdateObject(EnterpriseManagementObject enterpriseManagementObject, Dictionary<string, object> properties)
        {
            UpdateObject(new[] { enterpriseManagementObject }, properties);
        }
        public void UpdateObject(IEnumerable<EnterpriseManagementObject> enterpriseManagementObjects, Dictionary<string, object> properties, CancellationToken cancellationToken = default)
        {

            var groups = GroupIn10(enterpriseManagementObjects);

            foreach (var enumerable in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var idd = new IncrementalDiscoveryData();
                foreach (var dictionary in enumerable)
                {
                    var obj = UpdateEnterpriseManagementObjectWithRelations(dictionary, properties);
                    obj.EnterpriseManagementObject.Overwrite();
                    var rootId = AddRelatedObjects(obj, ref idd);
                    RemoveRelatedObjects(obj, ref idd);
                    RemoveRelationship(obj, ref idd);
                }
                idd.Commit(_client.ManagementGroup);
            }

        }

        private Dictionary<string, ManagementPackProperty> GetObjectPropertyDictionary(ManagementPackClass objectClass)
        {
            return _objectPropertyDictionary.GetOrAdd(objectClass.Id, guid =>
            {
                var dict = new Dictionary<string, ManagementPackProperty>();
                var objectProperties = objectClass.GetProperties(BaseClassTraversalDepth.Recursive);
                foreach (var managementPackProperty in objectProperties)
                {
                    if (!dict.ContainsKey(managementPackProperty.Name))
                    {
                        dict.Add(managementPackProperty.Name, managementPackProperty);
                    }
                }

                return dict;
            });

        }

        private IEnumerable<IEnumerable<T>> GroupIn10<T>(IEnumerable<T> enumerable)
        {
            var dictionaries = enumerable.ToList();
            var dictCount = dictionaries.Count;
            if (dictCount % 10 != 0)
                dictCount = (dictCount - dictCount % 10) + 10;

            var batchsize = dictCount >= 100 ? dictCount / 10 : 10;

            var groups = dictionaries.Select((x, idx) => new { x, idx })
                .GroupBy(x => x.idx / batchsize)
                .Select(g => g.Select(a => a.x));

            return groups;
        }

        private Guid AddIncremental(IWithRelations obj, ref IncrementalDiscoveryData incrementalDiscoveryData)
        {
            switch (obj)
            {
                case EnterpriseManagementObjectWithRelations enterpriseManagementObjectWithRelations:
                    {
                        incrementalDiscoveryData.Add(enterpriseManagementObjectWithRelations.EnterpriseManagementObject);
                        break;
                    }
                case EnterpriseManagementObjectProjectionWithRelations enterpriseManagementObjectProjectionWithRelations:
                    {
                        incrementalDiscoveryData.Add(enterpriseManagementObjectProjectionWithRelations.EnterpriseManagementObjectProjection);
                        break;
                    }
                default:
                    {
                        throw new NotSupportedException($"Type '{obj.GetType()}' not supported!");
                    }
            }


            if (obj.RelatedObjects != null)
            {
                foreach (var child in obj.RelatedObjects)
                {

                    AddIncremental(child, ref incrementalDiscoveryData);
                    incrementalDiscoveryData.Add(_client.Relations().buildCreatableEnterpriseManagementRelationshipObject(
                        obj.GetCoreEnterpriseManagementObject(), child.EnterpriseManagementObject));
                }
            }

            return obj.GetCoreEnterpriseManagementObject().Id;
        }

        private Guid AddRelatedObjects(EnterpriseManagementObjectWithRelations obj, ref IncrementalDiscoveryData incrementalDiscoveryData)
        {
            if (obj.RelatedObjects != null)
            {
                foreach (var child in obj.RelatedObjects)
                {

                    AddIncremental(child, ref incrementalDiscoveryData);
                    incrementalDiscoveryData.Add(_client.Relations().buildCreatableEnterpriseManagementRelationshipObject(
                        obj.EnterpriseManagementObject, child.EnterpriseManagementObject));
                }
            }

            return obj.EnterpriseManagementObject.Id;
        }

        private void RemoveRelatedObjects(EnterpriseManagementObjectWithRelations obj, ref IncrementalDiscoveryData incrementalDiscoveryData)
        {
            if (obj.RemoveRelatedObjects != null)
            {
                foreach (var child in obj.RemoveRelatedObjects)
                {
                    incrementalDiscoveryData.Remove(child);
                }
            }


        }

        private void RemoveRelationship(EnterpriseManagementObjectWithRelations obj, ref IncrementalDiscoveryData incrementalDiscoveryData)
        {
            if (obj.RemoveRelationShip != null)
            {
                foreach (var child in obj.RemoveRelationShip)
                {
                    incrementalDiscoveryData.Remove(child);
                }
            }


        }

        private EnterpriseManagementObjectWithRelations BuildEnterpriseManagementObjectWithRelations(
            string className, Dictionary<string, object> properties)
        {
            var objectClass = _client.Types().GetClassByName(className);
            return BuildEnterpriseManagementObjectWithRelations(objectClass, properties);
        }

        private EnterpriseManagementObjectWithRelations BuildEnterpriseManagementObjectWithRelations(
            ManagementPackClass objectClass, Dictionary<string, object> properties)
        {

            var obj = new CreatableEnterpriseManagementObject(_client.ManagementGroup, objectClass);
            var result = new EnterpriseManagementObjectWithRelations(obj);

            AddProperties(result, objectClass, properties);

            return result;
        }


        private EnterpriseManagementObjectWithRelations UpdateEnterpriseManagementObjectWithRelations(EnterpriseManagementObject enterpriseManagementObject, Dictionary<string, object> properties)
        {

            var objectClass = enterpriseManagementObject.GetManagementPackClass();
            var result = new EnterpriseManagementObjectWithRelations(enterpriseManagementObject);

            AddProperties(result, objectClass, properties);

            return result;
        }


        private EnterpriseManagementObjectProjectionWithRelations BuildEnterpriseManagementObjectProjectionWithRelations(
            ManagementPackObjectTemplate template, Dictionary<string, object> properties)
        {
            var elem = template.TypeID.GetElement();
            if (!(elem is ManagementPackTypeProjection managementPackTypeProjection))
            {
                throw new Exception($"Template '{template.DisplayName}' is invalid!");
            }

            var obj = new EnterpriseManagementObjectProjection(_client.ManagementGroup, template);
            var result = new EnterpriseManagementObjectProjectionWithRelations(obj);

            AddProperties(result, managementPackTypeProjection.TargetType, properties);

            return result;
        }

        private void AddProperties(IWithRelations objWithRelations, ManagementPackClass objectClass, Dictionary<string, object> properties, bool skipKeyProperties = false)
        {
            var entObj = objWithRelations.GetCoreEnterpriseManagementObject();
            var objectProperties = GetObjectPropertyDictionary(objectClass);
            var normalizer = new ValueConverter(_client);

            foreach (var kv in properties)
            {
                var name = kv.Key;
                var value = kv.Value;

                var mode = UpdateMode.Set;

                if (name.EndsWith("--"))
                {
                    mode = UpdateMode.ForceRemove;
                    name = name.Substring(0, name.Length - 2);
                }
                else if (name.EndsWith("-"))
                {
                    mode = UpdateMode.Remove;
                    name = name.Substring(0, name.Length - 1);
                }
                else if (name.EndsWith("+"))
                {
                    mode = UpdateMode.Add;
                    name = name.Substring(0, name.Length - 1);
                }




                if (name.Contains("!"))
                {
                    string className = null;
                    string propertyName = null;

                    var splittedName = name.Split("!".ToCharArray(), 2);
                    className = splittedName[0];
                    if (splittedName.Length > 1)
                    {
                        propertyName = splittedName[1]?.Trim().ToNull();
                    }

                    if (value?.GetType().IsEnumerableType(false) != true)
                    {
                        value = new List<object> { value };
                    }

                    var enu = value as IEnumerable;

                    var relationsBefore = _client.Relations().GetRelationshipObjectsByClassName(entObj.Id, className).ToList();

                    foreach (var o in enu)
                    {
                        var oVal = o;
                        var itemClassName = className;

                        if (mode == UpdateMode.Remove && oVal == null)
                        {
                            foreach (var rel in relationsBefore)
                            {
                                objWithRelations.RemoveRelationship(rel);
                            }
                            continue;
                        }

                        if (!String.IsNullOrWhiteSpace(propertyName))
                        {
                            var foundobj = GetEnterpriseManagementObjectsByClassName(itemClassName, $"{propertyName} -eq '{oVal}'", 1).FirstOrDefault();
                            if (foundobj == null)
                            {
                                continue;
                            }
                            oVal = foundobj.Id;
                        }


                        if (oVal is JObject jObject)
                        {
                            if (mode == UpdateMode.Remove)
                            {
                                throw new NotSupportedException("Object for Removing is not supported!");
                            }
                            oVal = Json.Converter.ToDictionary(jObject);
                        }

                        if (oVal is JToken jt)
                        {
                            oVal = Json.Converter.ToBasicDotNetObject(jt);
                        }

                        if (oVal is string str)
                        {
                            if (str.IsGuid())
                            {
                                oVal = str.ToGuid();
                            }
                        }

                        switch (oVal)
                        {

                            case Dictionary<string, object> dict:
                                {
                                    if (mode == UpdateMode.Remove || mode == UpdateMode.ForceRemove)
                                    {
                                        throw new NotSupportedException("Object for Removing is not supported!");
                                    }

                                    if (dict.ContainsKey("~type"))
                                    {
                                        itemClassName = dict["~type"].ToString();
                                    }


                                    var newRelation = BuildEnterpriseManagementObjectWithRelations(itemClassName, dict);
                                    objWithRelations.AddRelatedObject(newRelation);
                                    relationsBefore = relationsBefore
                                        .Where(r => r.Id != newRelation.EnterpriseManagementObject?.Id).ToList();
                                    break;
                                }
                            case Guid guid:
                                {


                                    switch (mode)
                                    {
                                        case UpdateMode.Remove:
                                            {

                                                var rel = relationsBefore.FirstOrDefault(r =>
                                                    {
                                                        if (r.SourceObject.Id == entObj.Id)
                                                        {
                                                            return r.TargetObject.Id == guid;
                                                        }
                                                        else
                                                        {
                                                            return r.SourceObject.Id == guid;
                                                        }

                                                    });

                                                if (rel != null)
                                                {
                                                    objWithRelations.RemoveRelationship(rel);
                                                }

                                                break;
                                            }
                                        case UpdateMode.ForceRemove:
                                            {
                                                var existingObject = GetEnterpriseManagementObjectById(guid);
                                                objWithRelations.RemoveRelatedObject(existingObject);
                                                break;
                                            }
                                        case UpdateMode.Add:
                                            {
                                                var existingObject = GetEnterpriseManagementObjectById(guid);
                                                var related = new EnterpriseManagementObjectWithRelations(existingObject);
                                                objWithRelations.AddRelatedObject(related);
                                                relationsBefore = relationsBefore
                                                    .Where(r => r.Id != related.EnterpriseManagementObject?.Id).ToList();
                                                break;
                                            }
                                        case UpdateMode.Set:

                                            {
                                                var rel = relationsBefore.FirstOrDefault(r =>
                                                {
                                                    if (r.SourceObject.Id == entObj.Id)
                                                    {
                                                        return r.TargetObject.Id == guid;
                                                    }
                                                    else
                                                    {
                                                        return r.SourceObject.Id == guid;
                                                    }

                                                });

                                                if (rel == null)
                                                {
                                                    var existingObject = GetEnterpriseManagementObjectById(guid);
                                                    var related = new EnterpriseManagementObjectWithRelations(existingObject);
                                                    objWithRelations.AddRelatedObject(related);
                                                }
                                                else
                                                {
                                                    relationsBefore = relationsBefore
                                                        .Where(r => r.Id != rel?.Id).ToList();
                                                }


                                                break;
                                            }

                                    }

                                    break;
                                }
                        }

                    }


                    if (mode == UpdateMode.Set)
                    {
                        foreach (var enterpriseManagementRelationshipObject in relationsBefore)
                        {
                            objWithRelations.RemoveRelationship(enterpriseManagementRelationshipObject);
                        }
                    }

                    continue;
                }


                if (objectProperties.TryGetValue(name, out var prop))
                {
                    if (skipKeyProperties && prop.Key)
                        continue;

                    if (mode == UpdateMode.Remove)
                    {
                        if (String.IsNullOrWhiteSpace(prop.DefaultValue))
                        {
                            entObj[objectClass, name].Value = null;
                        }
                        else
                        {
                            entObj[objectClass, name].SetToDefault();
                        }

                    }
                    else
                    {
                        if (value == null)
                            continue;

                        var val = normalizer.NormalizeValue(kv.Value, prop);
                        entObj[objectClass, name].Value = val;

                    }


                }



            }
        }

    }
}
