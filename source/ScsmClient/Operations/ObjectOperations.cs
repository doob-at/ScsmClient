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
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Reflectensions.ExtensionMethods;
using ScsmClient.Attributes;
using ScsmClient.ExtensionMethods;
using ScsmClient.Helper;
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

        internal EnterpriseManagementObject GetEnterpriseManagementObjectById(Guid id)
        {
            var critOptions = new ObjectQueryOptions();
            critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.NonBuffered;

            return _client.ManagementGroup.EntityObjects.GetObject<EnterpriseManagementObject>(id, critOptions);
        }

        public ScsmObject GetObjectById(Guid id)
        {
            return GetEnterpriseManagementObjectById(id).ToScsmObject();
        }

        public IEnumerable<ScsmObject> GetObjectsByClassName(string className, string criteria, int? maxResult = null)
        {
            var objectClass = _client.Class().GetClassByName(className);
            return GetObjectsByClass(objectClass, criteria, maxResult);
        }

        public IEnumerable<ScsmObject> GetObjectsByClassId(Guid classId, string criteria, int? maxResult = null)
        {
            var objectClass = _client.Class().GetClassById(classId);
            return GetObjectsByClass(objectClass, criteria, maxResult);
        }

        public IEnumerable<ScsmObject> GetObjectsByClass(ManagementPackClass objectClass, string criteria, int? maxResult = null)
        {


            var crit = _client.Criteria().BuildObjectCriteria(criteria, objectClass);


            var critOptions = new ObjectQueryOptions();
            critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.NonBuffered;
            critOptions.MaxResultCount = maxResult ?? Int32.MaxValue;


            var reader = _client.ManagementGroup.EntityObjects.GetObjectReader<EnterpriseManagementObject>(crit, critOptions);

            var count = 0;

            foreach (EnterpriseManagementObject enterpriseManagementObject in reader)
            {
                if (count == critOptions.MaxResultCount)
                    break;
                yield return enterpriseManagementObject.ToScsmObject();
            }

        }


        public Guid CreateObjectByClassId(Guid id, Dictionary<string, object> properties)
        {
            var objectClass = _client.Class().GetClassById(id);
            return CreateObjectByClass(objectClass, properties);
        }

        public Guid CreateObjectByClassName(string className, Dictionary<string, object> properties)
        {
            var objectClass = _client.Class().GetClassByName(className);
            return CreateObjectByClass(objectClass, properties);
        }

        public Guid CreateObjectByClass(ManagementPackClass objectClass, Dictionary<string, object> properties)
        {

            var obj = buildCreatableEnterpriseManagementObject(objectClass, properties);

            var idd = new IncrementalDiscoveryData();

            var rootId = AddIncremental(obj, ref idd);

            idd.Commit(_client.ManagementGroup);

            return rootId;

        }

        private Guid AddIncremental(CreatableEnterpriseManagementObjectWithRelations obj, ref IncrementalDiscoveryData incrementalDiscoveryData)
        {
            incrementalDiscoveryData.Add(obj.CreatableEnterpriseManagementObject);
            if (obj.RelatedObjects != null)
            {
                foreach (var child in obj.RelatedObjects)
                {

                    AddIncremental(child, ref incrementalDiscoveryData);
                    incrementalDiscoveryData.Add(_client.Relations().buildCreatableEnterpriseManagementRelationshipObject(
                        obj.CreatableEnterpriseManagementObject, child.CreatableEnterpriseManagementObject));
                }
            }

            return obj.CreatableEnterpriseManagementObject.Id;
        }

        private CreatableEnterpriseManagementObjectWithRelations buildCreatableEnterpriseManagementObject(
            string className, Dictionary<string, object> properties)
        {
            var objectClass = _client.Class().GetClassByName(className);
            return buildCreatableEnterpriseManagementObject(objectClass, properties);
        }

        private CreatableEnterpriseManagementObjectWithRelations buildCreatableEnterpriseManagementObject(
            ManagementPackClass objectClass, Dictionary<string, object> properties)
        {

            
            var objectProperties = GetObjectPropertyDictionary(objectClass);
            var normalizer = new ValueConverter(_client);

            var obj = new CreatableEnterpriseManagementObject(_client.ManagementGroup, objectClass);
            var result = new CreatableEnterpriseManagementObjectWithRelations(obj);

            foreach (var kv in properties)
            {
                var name = kv.Key;
                var value = kv.Value;
                if (name.StartsWith("!"))
                {
                    var className = name.Substring(1);

                    if (!value.GetType().IsEnumerableType(false))
                    {
                        value = new List<object>{value};
                    } 

                    if (value.GetType().IsEnumerableType(false))
                    {
                        var enu = value as IEnumerable;
                        foreach (var o in enu)
                        {
                            switch (o)
                            {
                                case Dictionary<string, object> dict:
                                {
                                    result.AddRelatedObject(buildCreatableEnterpriseManagementObject(className, dict));
                                    break;
                                }
                                case Guid guid:
                                {
                                    var existingObject = _client.Object().GetEnterpriseManagementObjectById(guid);
                                    var related = new CreatableEnterpriseManagementObjectWithRelations(existingObject);
                                    result.AddRelatedObject(related);
                                    break;
                                }
                                case string str:
                                {
                                    var g = str.ToGuid();
                                    var existingObject = _client.Object().GetEnterpriseManagementObjectById(g);
                                    var related = new CreatableEnterpriseManagementObjectWithRelations(existingObject);
                                    result.AddRelatedObject(related);
                                    break;
                                }
                            }

                        }
                    }
                    

                    //if (value.GetType().IsEnumerableType(false))
                    //{
                        
                    //}
                    //else
                    //{

                    //}
                    //if (value is Dictionary<string, object> dict)
                    //{
                    //    var className = name.Trim('!');
                    //    result.AddRelatedObject(buildCreatableEnterpriseManagementObject(className, dict));
                    //} 
                    
                }

                if (objectProperties.TryGetValue(name, out var prop))
                {
                    var val = normalizer.NormalizeValue(kv.Value, prop);
                    obj[objectClass, name].Value = val;
                }
            }

            return result;
        }

        public Dictionary<int, Guid> CreateObjectsByClassId(Guid id, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var objectClass = _client.Class().GetClassById(id);
            return CreateObjectsByClass(objectClass, objects, cancellationToken);
        }

        public Dictionary<int, Guid> CreateObjectsByClassName(string className, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var objectClass = _client.Class().GetClassByName(className);
            return CreateObjectsByClass(objectClass, objects, cancellationToken);
        }

        public Dictionary<int, Guid> CreateObjectsByClass(ManagementPackClass objectClass, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var result = new Dictionary<int, Guid>();
            var objectProperties = GetObjectPropertyDictionary(objectClass);
            var normalizer = new ValueConverter(_client);

            var groups = GroupIn10(objects);

            var index = 0;
            foreach (var enumerable in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var _list = new List<Guid>();
                var idd = new IncrementalDiscoveryData();
                foreach (var dictionary in enumerable)
                {
                    var obj = buildCreatableEnterpriseManagementObject(objectClass, dictionary);
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



        public Guid CreateObjectFromTemplateName(string templateName, Dictionary<string, object> properties)
        {

            var template = _client.Template().GetObjectTemplateByName(templateName);
            return CreateObjectFromTemplate(template, properties);
        }

        public Guid CreateObjectFromTemplate(ManagementPackObjectTemplate template, Dictionary<string, object> properties)
        {

            var obj = new EnterpriseManagementObjectProjection(_client.ManagementGroup, template);
            var normalizer = new ValueConverter(_client);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                var objectProperties = GetObjectPropertyDictionary(managementPackTypeProjection.TargetType);
                foreach (var kv in properties)
                {
                    if (objectProperties.TryGetValue(kv.Key, out var prop))
                    {
                        var val = normalizer.NormalizeValue(kv.Value, prop);
                        obj.Object[managementPackTypeProjection.TargetType, kv.Key].Value = val;
                    }
                }

                obj.Commit();

                return obj.Object.Id;
            }
            else
            {
                throw new Exception($"Template '{template.DisplayName}' is invalid!");
            }


        }


        public Dictionary<int, Guid> CreateObjectsFromTemplateName(string templateName, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var template = _client.Template().GetObjectTemplateByName(templateName);
            return CreateObjectsFromTemplate(template, objects, cancellationToken);
        }

        public Dictionary<int, Guid> CreateObjectsFromTemplate(ManagementPackObjectTemplate template, IEnumerable<Dictionary<string, object>> objects, CancellationToken cancellationToken = default)
        {

            var result = new Dictionary<int, Guid>();
            var normalizer = new ValueConverter(_client);

            var elem = template.TypeID.GetElement();
            if (!(elem is ManagementPackTypeProjection managementPackTypeProjection))
            {
                throw new Exception($"Template '{template.DisplayName}' is invalid!");
            }


            var groups = GroupIn10(objects);

            var index = 0;
            foreach (var enumerable in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var _list = new List<Guid>();
                var idd = new IncrementalDiscoveryData();
                foreach (var dictionary in enumerable)
                {
                    var obj = new EnterpriseManagementObjectProjection(_client.ManagementGroup, template);
                    var objectProperties = GetObjectPropertyDictionary(managementPackTypeProjection.TargetType);
                    foreach (var kv in dictionary)
                    {
                        if (objectProperties.TryGetValue(kv.Key, out var prop))
                        {
                            var val = normalizer.NormalizeValue(kv.Value, prop);
                            obj.Object[managementPackTypeProjection.TargetType, kv.Key].Value = val;
                        }
                    }

                    idd.Add(obj);
                    _list.Add(obj.Object.Id);
                }
                idd.Commit(_client.ManagementGroup);
                foreach (var guid in _list)
                {
                    result.Add(index++, guid);
                }
            }

            return result;

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
    }

    public class CreatableEnterpriseManagementObjectWithRelations
    {
        public EnterpriseManagementObject CreatableEnterpriseManagementObject { get; }

        public List<CreatableEnterpriseManagementObjectWithRelations> RelatedObjects { get; set; }

        public CreatableEnterpriseManagementObjectWithRelations(EnterpriseManagementObject creatableEnterpriseManagementObject)
        {
            CreatableEnterpriseManagementObject = creatableEnterpriseManagementObject;
        }

        public CreatableEnterpriseManagementObjectWithRelations AddRelatedObject(
            CreatableEnterpriseManagementObjectWithRelations creatableEnterpriseManagementObject)
        {
            if (RelatedObjects == null)
            {
                RelatedObjects = new List<CreatableEnterpriseManagementObjectWithRelations>();
            }
            RelatedObjects.Add(creatableEnterpriseManagementObject);
            return this;
        }
    }

}
