﻿using System;
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
using doob.Reflectensions;
using doob.Reflectensions.Common;
using doob.Reflectensions.ExtensionMethods;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Newtonsoft.Json.Linq;
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

        private ConcurrentDictionary<Guid, EnterpriseManagementObject> _cachedObjects = new ConcurrentDictionary<Guid, EnterpriseManagementObject>();
        private ConcurrentDictionary<string, EnterpriseManagementObject> _cachedSearchedObjects = new ConcurrentDictionary<string, EnterpriseManagementObject>();


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

        public List<EnterpriseManagementObject> GetEnterpriseManagementObjectsByIds(IEnumerable<Guid> ids)
        {
            var critOptions = new ObjectQueryOptions();
            critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.NonBuffered;

            return _client.ManagementGroup.EntityObjects
                .GetObjectReader<EnterpriseManagementObject>(ids.ToList(), critOptions).ToList();
        }

        public List<EnterpriseManagementObject> GetEnterpriseManagementObjectsByClassName(string className, string criteria, RetrievalOptions retrievalOptions = null)
        {
            var objectClass = _client.Types().GetClassByName(className);
            return GetEnterpriseManagementObjectsByClass(objectClass, criteria, retrievalOptions);
        }

        public List<EnterpriseManagementObject> GetEnterpriseManagementObjectsByClassId(Guid classId, string criteria, RetrievalOptions retrievalOptions = null)
        {
            var objectClass = _client.Types().GetClassById(classId);
            return GetEnterpriseManagementObjectsByClass(objectClass, criteria, retrievalOptions);
        }

        //public IEnumerable<EnterpriseManagementObject> GetEnterpriseManagementObjectsByClass(ManagementPackClass objectClass, string criteria, int? maxResult = null)
        //{


        //    var crit = _client.Criteria().BuildObjectCriteria(criteria, objectClass);


        //    var critOptions = new ObjectQueryOptions();

        //    critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
        //    if (maxResult.HasValue && maxResult.Value != int.MaxValue)
        //    {
        //        critOptions.MaxResultCount = maxResult.Value;
        //        critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.Buffered;
        //    }

        //    var sortprop = new EnterpriseManagementObjectGenericProperty(EnterpriseManagementObjectGenericPropertyName.TimeAdded);
        //    critOptions.AddSortProperty(sortprop, SortingOrder.Ascending);


        //    var reader = _client.ManagementGroup.EntityObjects.GetObjectReader<EnterpriseManagementObject>(crit, critOptions);


        //    foreach (EnterpriseManagementObject enterpriseManagementObject in reader)
        //    {
        //        yield return enterpriseManagementObject;
        //    }

        //}

        public List<EnterpriseManagementObject> GetEnterpriseManagementObjectsByClass(ManagementPackClass objectClass, string criteria, RetrievalOptions retrievalOptions = null)
        {

            retrievalOptions = retrievalOptions ?? new RetrievalOptions();

            var crit = _client.Criteria().BuildObjectCriteria(criteria, objectClass);


            var critOptions = new ObjectQueryOptions();
            critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.Buffered;

            if (retrievalOptions.PropertiesToLoad != null)
            {
                critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.None;
                if (retrievalOptions.PropertiesToLoad.Any())
                {
                    var objectClassProperties = objectClass.GetProperties(BaseClassTraversalDepth.Recursive);
                    foreach (var s in retrievalOptions.PropertiesToLoad)
                    {
                        var prop = objectClassProperties.FirstOrDefault(ocp => ocp.Name.Equals(s));
                        if (prop != null)
                        {
                            critOptions.AddPropertyToRetrieve(objectClass, prop);

                        }
                    }
                }
            }
            else
            {
                critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            }


            if (retrievalOptions.MaxResultCount.HasValue && retrievalOptions.MaxResultCount.Value != int.MaxValue)
            {
                critOptions.MaxResultCount = retrievalOptions.MaxResultCount.Value;
            }

            var sortprop = new EnterpriseManagementObjectGenericProperty(EnterpriseManagementObjectGenericPropertyName.TimeAdded);
            critOptions.AddSortProperty(sortprop, SortingOrder.Ascending);


            var reader = _client.ManagementGroup.EntityObjects.GetObjectReader<EnterpriseManagementObject>(crit, critOptions);
            return reader.ToList();
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
            return CreateObjectsByClass(objectClass, new[] { properties }).FirstOrDefault().Value;
        }


        public Dictionary<int, Guid> CreateObjectsByClassId(Guid id, IEnumerable<Dictionary<string, object>> objects, CreateOptions createOptions = null, CancellationToken cancellationToken = default)
        {

            var objectClass = _client.Types().GetClassById(id);
            return CreateObjectsByClass(objectClass, objects, createOptions, cancellationToken);
        }


        public Dictionary<int, Guid> CreateObjectsByClassName(string typeName, IEnumerable<Dictionary<string, object>> objects, CreateOptions createOptions = null, CancellationToken cancellationToken = default)
        {

            var objectClass = _client.Types().GetClassByName(typeName);
            if (objectClass != null)
            {
                return CreateObjectsByClass(objectClass, objects, createOptions, cancellationToken);
            }

            var typeProjectionClass = _client.Types().GetTypeProjectionByName(typeName);
            if (typeProjectionClass != null)
            {
                return CreateObjectsByTypeProjection(typeProjectionClass, objects, createOptions, cancellationToken);
            }

            throw new ObjectNotFoundException($"Can't find a ManagementPackClass nor a TypeProjection with the name '{typeName}'");

           
            
        }


        public Dictionary<int, Guid> CreateObjectsByClass(ManagementPackClass objectClass, IEnumerable<Dictionary<string, object>> objects, CreateOptions createOptions = null, CancellationToken cancellationToken = default)
        {

            createOptions = createOptions ?? new CreateOptions();
            createOptions.BatchSize = createOptions.BatchSize ?? 1000;

            if (createOptions.BuildCacheForObjects != null)
            {
                foreach (var buildCacheForObject in createOptions.BuildCacheForObjects)
                {
                    foreach (var enterpriseManagementObject in GetEnterpriseManagementObjectsByClassName(buildCacheForObject.Key, buildCacheForObject.Value))
                    {
                        _cachedObjects.TryAdd(enterpriseManagementObject.Id, enterpriseManagementObject);
                    }
                }
            }

            var result = new Dictionary<int, Guid>();
            var index = 0;
            List<Guid> _list = new List<Guid>();
            IncrementalDiscoveryData idd = new IncrementalDiscoveryData();
            int currentCount = 0;
            foreach (var dictionary in objects)
            {


                var obj = BuildEnterpriseManagementObjectWithRelations(objectClass, dictionary);
                var rootId = AddIncremental(obj, ref idd);
                _list.Add(rootId);
                currentCount++;

                if (currentCount >= createOptions.BatchSize)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    currentCount = 0;
                    idd.Commit(_client.ManagementGroup);
                    foreach (var guid in _list)
                    {
                        result.Add(index++, guid);
                    }

                    _list = new List<Guid>();
                    idd = new IncrementalDiscoveryData();
                }


            }

            if (idd != null)
            {
                idd.Commit(_client.ManagementGroup);
                foreach (var guid in _list)
                {
                    result.Add(index++, guid);
                }
            }

            _list = null;
            idd = null;

            return result;

        }

        public Dictionary<int, Guid> CreateObjectsByTypeProjection(ManagementPackTypeProjection typeProjectionClass, IEnumerable<Dictionary<string, object>> objects, CreateOptions createOptions = null, CancellationToken cancellationToken = default)
        {

            createOptions = createOptions ?? new CreateOptions();
            createOptions.BatchSize = createOptions.BatchSize ?? 1000;

            if (createOptions.BuildCacheForObjects != null)
            {
                foreach (var buildCacheForObject in createOptions.BuildCacheForObjects)
                {
                    foreach (var enterpriseManagementObject in GetEnterpriseManagementObjectsByClassName(buildCacheForObject.Key, buildCacheForObject.Value))
                    {
                        _cachedObjects.TryAdd(enterpriseManagementObject.Id, enterpriseManagementObject);
                    }
                }
            }

            var result = new Dictionary<int, Guid>();
            var index = 0;
            List<Guid> _list = new List<Guid>();
            IncrementalDiscoveryData idd = new IncrementalDiscoveryData();
            int currentCount = 0;
            foreach (var dictionary in objects)
            {


                var obj = BuildEnterpriseManagementObjectWithRelations(typeProjectionClass, dictionary);
                var rootId = AddIncremental(obj, ref idd);
                _list.Add(rootId);
                currentCount++;

                if (currentCount >= createOptions.BatchSize)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    currentCount = 0;
                    idd.Commit(_client.ManagementGroup);
                    foreach (var guid in _list)
                    {
                        result.Add(index++, guid);
                    }

                    _list = new List<Guid>();
                    idd = new IncrementalDiscoveryData();
                }


            }

            if (idd != null)
            {
                idd.Commit(_client.ManagementGroup);
                foreach (var guid in _list)
                {
                    result.Add(index++, guid);
                }
            }

            _list = null;
            idd = null;

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

            return CreateObjectsFromTemplate(template, new[] { properties }).First().Value;
        }

        public Dictionary<int, Guid> CreateObjectsFromTemplateId(Guid id, IEnumerable<Dictionary<string, object>> objects, CreateOptions createOptions = null, CancellationToken cancellationToken = default)
        {

            var template = _client.Template().GetObjectTemplateById(id);
            return CreateObjectsFromTemplate(template, objects, createOptions, cancellationToken);
        }

        public Dictionary<int, Guid> CreateObjectsFromTemplateName(string templateName, IEnumerable<Dictionary<string, object>> objects, CreateOptions createOptions = null, CancellationToken cancellationToken = default)
        {

            var template = _client.Template().GetObjectTemplateByName(templateName);
            return CreateObjectsFromTemplate(template, objects, createOptions, cancellationToken);
        }

        public Dictionary<int, Guid> CreateObjectsFromTemplate(ManagementPackObjectTemplate template, IEnumerable<Dictionary<string, object>> objects, CreateOptions createOptions = null, CancellationToken cancellationToken = default)
        {
            createOptions = createOptions ?? new CreateOptions();
            createOptions.BatchSize = createOptions.BatchSize ?? 1000;

            if (createOptions.BuildCacheForObjects != null)
            {
                foreach (var buildCacheForObject in createOptions.BuildCacheForObjects)
                {
                    foreach (var enterpriseManagementObject in GetEnterpriseManagementObjectsByClassName(buildCacheForObject.Key, buildCacheForObject.Value))
                    {
                        _cachedObjects.TryAdd(enterpriseManagementObject.Id, enterpriseManagementObject);
                    }
                }
            }

            var result = new Dictionary<int, Guid>();
            var index = 0;
            List<Guid> _list = null;
            IncrementalDiscoveryData idd = null;
            int currentCount = 0;
            foreach (var dictionary in objects)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (idd == null)
                {
                    _list = new List<Guid>();
                    idd = new IncrementalDiscoveryData();
                }

                var obj = BuildEnterpriseManagementObjectProjectionWithRelations(template, dictionary);
                var rootId = AddIncremental(obj, ref idd);
                _list.Add(rootId);
                currentCount++;

                if (currentCount >= createOptions.BatchSize)
                {
                    currentCount = 0;
                    idd.Commit(_client.ManagementGroup);
                    foreach (var guid in _list)
                    {
                        result.Add(index++, guid);
                    }

                    _list = null;
                    idd = null;
                }


            }

            if (idd != null)
            {
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
            return DeleteObjectsByClassName(className, criteria, 1000, cancellationToken);
        }
        public int DeleteObjectsByClassName(string className, string criteria, int maxItemsPerTransaction, CancellationToken cancellationToken = default)
        {
            var obj = GetEnterpriseManagementObjectsByClassName(className, criteria, new RetrievalOptions
            {
                PropertiesToLoad = new List<string> { "Id" }
            });
            return DeleteObjects(obj, maxItemsPerTransaction, cancellationToken);
        }

        public int DeleteObjectsByClassId(Guid classId, string criteria, CancellationToken cancellationToken = default)
        {
            return DeleteObjectsByClassId(classId, criteria, 1000, cancellationToken);
        }
        public int DeleteObjectsByClassId(Guid classId, string criteria, int maxItemsPerTransaction, CancellationToken cancellationToken = default)
        {
            var obj = GetEnterpriseManagementObjectsByClassId(classId, criteria);
            return DeleteObjects(obj, maxItemsPerTransaction, cancellationToken);
        }

        public int DeleteObjectsByClass(ManagementPackClass objectClass, string criteria, CancellationToken cancellationToken = default)
        {
            return DeleteObjectsByClass(objectClass, criteria, 1000, cancellationToken);
        }
        public int DeleteObjectsByClass(ManagementPackClass objectClass, string criteria, int maxItemsPerTransaction, CancellationToken cancellationToken = default)
        {
            var obj = GetEnterpriseManagementObjectsByClass(objectClass, criteria);
            return DeleteObjects(obj, maxItemsPerTransaction, cancellationToken);
        }

        public int DeleteObjectsById(IEnumerable<Guid> objectIds, CancellationToken cancellationToken = default)
        {
            return DeleteObjectsById(objectIds, 1000, cancellationToken);
        }
        public int DeleteObjectsById(IEnumerable<Guid> objectIds, int maxItemsPerTransaction, CancellationToken cancellationToken = default)
        {
            var objs = GetEnterpriseManagementObjectsByIds(objectIds);

            return DeleteObjects(objs, maxItemsPerTransaction, cancellationToken);
        }

        public int DeleteObjects(IEnumerable<EnterpriseManagementObject> objects, CancellationToken cancellationToken = default)
        {
            return DeleteObjects(objects, 1000, cancellationToken);
        }
        public int DeleteObjects(IEnumerable<EnterpriseManagementObject> objects, int maxItemsPerTransaction, CancellationToken cancellationToken = default)
        {
            //var result = new Dictionary<int, Guid>();
            //var groups = GroupIn10(objects);

            var count = 0;
            IncrementalDiscoveryData idd = new IncrementalDiscoveryData();
            int currentCount = 0;
            foreach (var mgmtObject in objects)
            {
                cancellationToken.ThrowIfCancellationRequested();
                idd.Remove(mgmtObject);

                currentCount++;

                if (currentCount >= maxItemsPerTransaction)
                {
                    currentCount = 0;
                    idd.Commit(_client.ManagementGroup);
                    idd = new IncrementalDiscoveryData();
                }

                count++;
            }

            idd?.Commit(_client.ManagementGroup);

            return count;
        }




        public void SetObject(Guid id, Dictionary<string, object> properties)
        {
            var updDict = new Dictionary<Guid, Dictionary<string, object>>
            {
                [id] = properties
            };
            SetObjects(updDict);
        }

        public int SetObjects(Dictionary<Guid, Dictionary<string, object>> updateObjects,
            CancellationToken cancellationToken = default)
        {
            return SetObjects(updateObjects, 1000, cancellationToken);
        }

        public int SetObjects(Dictionary<Guid, Dictionary<string, object>> updateObjects, int maxItemsPerTransaction,
            CancellationToken cancellationToken = default)
        {
            return _UpdateObjects(updateObjects, maxItemsPerTransaction, false, cancellationToken);
        }

        public void UpdateObject(Guid id, Dictionary<string, object> properties)
        {
            var updDict = new Dictionary<Guid, Dictionary<string, object>>
            {
                [id] = properties
            };
            UpdateObjects(updDict);
        }

        public int UpdateObjects(Dictionary<Guid, Dictionary<string, object>> updateObjects, CancellationToken cancellationToken = default)
        {
            return UpdateObjects(updateObjects, 1000, cancellationToken);
        }
        
        public int UpdateObjects(Dictionary<Guid, Dictionary<string, object>> updateObjects, int maxItemsPerTransaction, CancellationToken cancellationToken = default)
        {
            return _UpdateObjects(updateObjects, maxItemsPerTransaction, true, cancellationToken);
        }
        
        private int _UpdateObjects(Dictionary<Guid, Dictionary<string, object>> updateObjects, int maxItemsPerTransaction, bool checkETag, CancellationToken cancellationToken = default)
        {


            var enterpriseObjects = GetEnterpriseManagementObjectsByIds(updateObjects.Keys.ToList())
                .ToDictionary(o => o, o => updateObjects[o.Id]);

            IncrementalDiscoveryData idd = null;
            int currentCount = 0;
            var allCount = 0;
            foreach (var dictionary in enterpriseObjects)
            {
                if (checkETag)
                {
                    if (!dictionary.Value.TryGetValue("ETag", out var updateETag))
                    {
                        throw new Exception("[ETag_missing]:ETag value is needed for Update!");
                    }
                    
                    var eTag = dictionary.Key.CalculateETag();
                    if (eTag != $"{updateETag}")
                    {
                        throw new Exception("[ETag_mismatch]:ETag value is different, looks like there was an update in between");
                    }
                }

                cancellationToken.ThrowIfCancellationRequested();
                if (idd == null)
                {
                    idd = new IncrementalDiscoveryData();
                }

                var obj = UpdateEnterpriseManagementObjectWithRelations(dictionary.Key, dictionary.Value);
                obj.EnterpriseManagementObject.Overwrite();
                var rootId = AddRelatedObjects(obj, ref idd);
                RemoveRelatedObjects(obj, ref idd);
                RemoveRelationship(obj, ref idd);

                currentCount++;
                allCount++;
                if (currentCount >= maxItemsPerTransaction)
                {
                    currentCount = 0;
                    idd.Commit(_client.ManagementGroup);
                    idd = null;
                }

            }

            idd?.Commit(_client.ManagementGroup);
            return allCount;
        }






        public void SetObject(string typeProjectionName, Guid id, Dictionary<string, object> properties)
        {
            var typePro = _client.Types().GetTypeProjectionByName(typeProjectionName);
            SetObject(typePro, id, properties);
        }

        public void SetObject(ManagementPackTypeProjection managementPackTypeProjection, Guid id, Dictionary<string, object> properties)
        {

            var updDict = new Dictionary<Guid, Dictionary<string, object>>
            {
                [id] = properties
            };
            SetObjects(managementPackTypeProjection, updDict);
        }


        public int SetObjects(string typeProjectionName, Dictionary<Guid, Dictionary<string, object>> updateObjects, CancellationToken cancellationToken = default)
        {
            var typePro = _client.Types().GetTypeProjectionByName(typeProjectionName);
            return SetObjects(typePro, updateObjects, 1000, cancellationToken);
        }

        public int SetObjects(ManagementPackTypeProjection managementPackTypeProjection, Dictionary<Guid, Dictionary<string, object>> updateObjects, CancellationToken cancellationToken = default)
        {
            return SetObjects(managementPackTypeProjection, updateObjects, 1000, cancellationToken);
        }

        public int SetObjects(ManagementPackTypeProjection managementPackTypeProjection, Dictionary<Guid, Dictionary<string, object>> updateObjects, int maxItemsPerTransaction, CancellationToken cancellationToken = default)
        {
            return _UpdateObjects(managementPackTypeProjection, updateObjects, maxItemsPerTransaction, false, cancellationToken);
        }






        public void UpdateObject(string typeProjectionName, Guid id,  Dictionary<string, object> properties)
        {
            var typePro = _client.Types().GetTypeProjectionByName(typeProjectionName);
            UpdateObject(typePro, id, properties);
        }

        public void UpdateObject(ManagementPackTypeProjection managementPackTypeProjection, Guid id, Dictionary<string, object> properties)
        {

            var updDict = new Dictionary<Guid, Dictionary<string, object>>
            {
                [id] = properties
            };
            UpdateObjects(managementPackTypeProjection, updDict);
        }


        public int UpdateObjects(string typeProjectionName, Dictionary<Guid, Dictionary<string, object>> updateObjects, CancellationToken cancellationToken = default)
        {
            var typePro = _client.Types().GetTypeProjectionByName(typeProjectionName);
            return UpdateObjects(typePro, updateObjects, 1000, cancellationToken);
        }

        public int UpdateObjects(ManagementPackTypeProjection managementPackTypeProjection, Dictionary<Guid, Dictionary<string, object>> updateObjects, CancellationToken cancellationToken = default)
        {
            return UpdateObjects(managementPackTypeProjection, updateObjects, 1000, cancellationToken);
        }

        public int UpdateObjects(ManagementPackTypeProjection managementPackTypeProjection, Dictionary<Guid, Dictionary<string, object>> updateObjects, int maxItemsPerTransaction, CancellationToken cancellationToken = default)
        {
            return _UpdateObjects(managementPackTypeProjection, updateObjects, maxItemsPerTransaction, true,
                cancellationToken);
        }


        private int _UpdateObjects(ManagementPackTypeProjection managementPackTypeProjection, Dictionary<Guid, Dictionary<string, object>> updateObjects, int maxItemsPerTransaction, bool checkETag, CancellationToken cancellationToken = default)
        {


            var enterpriseObjects = GetEnterpriseManagementObjectsByIds(updateObjects.Keys.ToList())
                .ToDictionary(o => o, o => updateObjects[o.Id]);


            IncrementalDiscoveryData idd = null;
            int currentCount = 0;
            var allCount = 0;
            foreach (var dictionary in enterpriseObjects)
            {
                if (checkETag)
                {
                    if (!dictionary.Value.TryGetValue("ETag", out var updateETag))
                    {
                        throw new Exception("[ETag_missing]:ETag value is needed for Update!");
                    }

                    var eTag = dictionary.Key.CalculateETag();
                    if (eTag != $"{updateETag}")
                    {
                        throw new Exception("[ETag_mismatch]:ETag value is different, looks like there was an update in between");
                    }
                }

                cancellationToken.ThrowIfCancellationRequested();
                if (idd == null)
                {
                    idd = new IncrementalDiscoveryData();
                }

                var obj = UpdateEnterpriseManagementObjectWithRelations(managementPackTypeProjection, dictionary.Key, dictionary.Value);
                obj.EnterpriseManagementObject.Overwrite();
                var rootId = AddRelatedObjects(obj, ref idd);
                RemoveRelatedObjects(obj, ref idd);
                RemoveRelationship(obj, ref idd);

                currentCount++;
                allCount++;
                if (currentCount >= maxItemsPerTransaction)
                {
                    currentCount = 0;
                    idd.Commit(_client.ManagementGroup);
                    idd = null;
                }

            }

            idd?.Commit(_client.ManagementGroup);
            return allCount;
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



            foreach (var child in obj.RelatedObjects)
            {

                AddIncremental(child, ref incrementalDiscoveryData);
                if (child.Relationship != null)
                {
                    incrementalDiscoveryData.Add(_client.Relations().buildCreatableEnterpriseManagementRelationshipObject(child.Relationship,
                        obj.GetCoreEnterpriseManagementObject(), child.EnterpriseManagementObject));
                }
                else
                {
                    incrementalDiscoveryData.Add(_client.Relations().buildCreatableEnterpriseManagementRelationshipObject(
                        obj.GetCoreEnterpriseManagementObject(), child.EnterpriseManagementObject));
                }

            }


            return obj.GetCoreEnterpriseManagementObject().Id;
        }

        private Guid AddRelatedObjects(EnterpriseManagementObjectWithRelations obj, ref IncrementalDiscoveryData incrementalDiscoveryData)
        {

            foreach (var child in obj.RelatedObjects)
            {

                AddIncremental(child, ref incrementalDiscoveryData);
                if (child.Relationship != null)
                {
                    incrementalDiscoveryData.Add(_client.Relations().buildCreatableEnterpriseManagementRelationshipObject(child.Relationship,
                        obj.GetCoreEnterpriseManagementObject(), child.EnterpriseManagementObject));
                }
                else
                {
                    incrementalDiscoveryData.Add(_client.Relations().buildCreatableEnterpriseManagementRelationshipObject(
                        obj.GetCoreEnterpriseManagementObject(), child.EnterpriseManagementObject));
                }

                //incrementalDiscoveryData.Add(_client.Relations().buildCreatableEnterpriseManagementRelationshipObject( obj.Relationship,
                //    obj.EnterpriseManagementObject, child.EnterpriseManagementObject));
            }


            return obj.EnterpriseManagementObject.Id;
        }

        private void RemoveRelatedObjects(EnterpriseManagementObjectWithRelations obj, ref IncrementalDiscoveryData incrementalDiscoveryData)
        {

            foreach (var child in obj.RemoveRelatedObjects)
            {
                incrementalDiscoveryData.Remove(child);
            }



        }

        private void RemoveRelationship(EnterpriseManagementObjectWithRelations obj, ref IncrementalDiscoveryData incrementalDiscoveryData)
        {

            foreach (var child in obj.RemoveRelationShip)
            {
                incrementalDiscoveryData.Remove(child);
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

            AddPropertiesForCreate(result, objectClass, properties);

            return result;
        }

        private EnterpriseManagementObjectWithRelations BuildEnterpriseManagementObjectWithRelations(ManagementPackTypeProjection typeProjectionClass, Dictionary<string, object> properties)
        {
            var obj = new CreatableEnterpriseManagementObject(_client.ManagementGroup, typeProjectionClass.TargetType);
            var result = new EnterpriseManagementObjectWithRelations(obj);

            AddPropertiesForCreate(result, typeProjectionClass, properties);

            return result;
        }




        private EnterpriseManagementObjectWithRelations UpdateEnterpriseManagementObjectWithRelations(EnterpriseManagementObject enterpriseManagementObject, Dictionary<string, object> properties)
        {

            var objectClass = enterpriseManagementObject.GetManagementPackClass();
            var result = new EnterpriseManagementObjectWithRelations(enterpriseManagementObject);

            AddProperties(result, objectClass, properties, true);

            return result;
        }

        private EnterpriseManagementObjectWithRelations UpdateEnterpriseManagementObjectWithRelations(ManagementPackTypeProjection managementPackTypeProjection, EnterpriseManagementObject enterpriseManagementObject, Dictionary<string, object> properties)
        {

            var result = new EnterpriseManagementObjectWithRelations(enterpriseManagementObject);

            AddProperties(result, managementPackTypeProjection, properties, true);

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

            AddPropertiesForCreate(result, managementPackTypeProjection.TargetType, properties);

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

                    if (value?.GetType().IsEnumerableType() != true)
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
                            var foundobj = _cachedSearchedObjects.GetOrAdd($"{itemClassName}|{propertyName} -eq '{oVal}'", s =>
                            {
                                var splitted = s.Split("|".ToCharArray(), 2);
                                return GetEnterpriseManagementObjectsByClassName(splitted[0], splitted[1], new RetrievalOptions() { MaxResultCount = 1 }).FirstOrDefault();
                            });

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

                                    if (dict.ContainsKey("@class"))
                                    {
                                        itemClassName = dict["@class"].ToString();
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
                                                var existingObject = _cachedObjects.GetOrAdd(guid, GetEnterpriseManagementObjectById);
                                                objWithRelations.RemoveRelatedObject(existingObject);
                                                break;
                                            }
                                        case UpdateMode.Add:
                                            {
                                                var existingObject = _cachedObjects.GetOrAdd(guid, GetEnterpriseManagementObjectById);
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
                                                    var existingObject = _cachedObjects.GetOrAdd(guid, GetEnterpriseManagementObjectById);
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

        private void AddProperties(IWithRelations objWithRelations, ManagementPackTypeProjection managementPackTypeProjection, Dictionary<string, object> properties, bool skipKeyProperties = false)
        {
            var entObj = objWithRelations.GetCoreEnterpriseManagementObject();
            var objectProperties = GetObjectPropertyDictionary(managementPackTypeProjection.TargetType);
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

                    if (value?.GetType().IsEnumerableType() != true)
                    {
                        value = new List<object> { value };
                    }

                    var enu = value as IEnumerable;

                    var trel = managementPackTypeProjection.FirstOrDefault(p => p.Value.Alias == className);
                    if (trel.Value == null)
                    {
                        trel = managementPackTypeProjection.FirstOrDefault(p => p.Key.ParentElement.Name == className);
                    }

                    if (trel.Value == null)
                    {
                        throw new Exception(
                            $"Can't find a TypeProjection with Alias '{className}' or Relationship with Id '{className}'");
                    }

                    var relationShip = trel.Key.ParentElement as ManagementPackRelationship;

                    className = trel.Value.TargetType.Name;

                    List<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> relationsBefore = _client.Relations().GetRelationshipObjectsByPropertyName(entObj.Id, relationShip).ToList();

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
                            var foundobj = _cachedSearchedObjects.GetOrAdd($"{itemClassName}|{propertyName} -eq '{oVal}'", s =>
                            {
                                var splitted = s.Split("|".ToCharArray(), 2);
                                return GetEnterpriseManagementObjectsByClassName(splitted[0], splitted[1], new RetrievalOptions() { MaxResultCount = 1 }).FirstOrDefault();
                            });

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

                                    if (dict.ContainsKey("@class"))
                                    {
                                        itemClassName = dict["@class"].ToString();
                                    }


                                    
                                    var newRelation = BuildEnterpriseManagementObjectWithRelations(itemClassName, dict);
                                    newRelation.Relationship = relationShip;
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
                                                var existingObject = _cachedObjects.GetOrAdd(guid, GetEnterpriseManagementObjectById);
                                                objWithRelations.RemoveRelatedObject(existingObject);
                                                break;
                                            }
                                        case UpdateMode.Add:
                                            {
                                                var existingObject = _cachedObjects.GetOrAdd(guid, GetEnterpriseManagementObjectById);
                                                var related = new EnterpriseManagementObjectWithRelations(existingObject);
                                                related.Relationship = relationShip;
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
                                                    var existingObject = _cachedObjects.GetOrAdd(guid, GetEnterpriseManagementObjectById);
                                                    var related = new EnterpriseManagementObjectWithRelations(existingObject);
                                                    related.Relationship = relationShip;
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
                            entObj[managementPackTypeProjection.TargetType, name].Value = null;
                        }
                        else
                        {
                            entObj[managementPackTypeProjection.TargetType, name].SetToDefault();
                        }

                    }
                    else
                    {
                        if (value == null)
                            continue;

                        var val = normalizer.NormalizeValue(kv.Value, prop);
                        entObj[managementPackTypeProjection.TargetType, name].Value = val;

                    }


                }



            }
        }




        private void AddPropertiesForCreate(IWithRelations objWithRelations, ManagementPackClass objectClass, Dictionary<string, object> properties, bool skipKeyProperties = false)
        {
            var entObj = objWithRelations.GetCoreEnterpriseManagementObject();
            var objectProperties = GetObjectPropertyDictionary(objectClass);
            var normalizer = new ValueConverter(_client);

            foreach (var kv in properties)
            {
                var name = kv.Key;
                var value = kv.Value;

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

                    if (value?.GetType().IsEnumerableType() != true)
                    {
                        value = new List<object> { value };
                    }

                    var enu = value as IEnumerable;

                    foreach (var o in enu)
                    {
                        var oVal = o;
                        var itemClassName = className;

                        if (!String.IsNullOrWhiteSpace(propertyName))
                        {
                            var foundobj = _cachedSearchedObjects.GetOrAdd($"{itemClassName}|{propertyName} -eq '{oVal}'", s =>
                            {
                                var splitted = s.Split("|".ToCharArray(), 2);
                                return GetEnterpriseManagementObjectsByClassName(splitted[0], splitted[1], new RetrievalOptions() { MaxResultCount = 1 }).FirstOrDefault();
                            });

                            //var foundobj = GetEnterpriseManagementObjectsByClassName(itemClassName, $"{propertyName} -eq '{oVal}'", new RetrievalOptions() {MaxResultCount = 1}).FirstOrDefault();
                            if (foundobj == null)
                            {
                                continue;
                            }
                            oVal = foundobj.Id;
                        }


                        if (oVal is JObject jObject)
                        {
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
                                    if (dict.ContainsKey("@class"))
                                    {
                                        itemClassName = dict["@class"].ToString();
                                    }


                                    var newRelation = BuildEnterpriseManagementObjectWithRelations(itemClassName, dict);
                                    objWithRelations.AddRelatedObject(newRelation);
                                    break;
                                }
                            case Guid guid:
                                {

                                    var existingObject = _cachedObjects.GetOrAdd(guid, GetEnterpriseManagementObjectById);
                                    var related = new EnterpriseManagementObjectWithRelations(existingObject);
                                    objWithRelations.AddRelatedObject(related);
                                    break;
                                }
                        }

                    }

                    continue;
                }


                if (objectProperties.TryGetValue(name, out var prop))
                {
                    if (skipKeyProperties && prop.Key)
                        continue;

                    if (value == null)
                        continue;

                    var val = normalizer.NormalizeValue(kv.Value, prop);
                    entObj[objectClass, name].Value = val;


                }



            }
        }


        private void AddPropertiesForCreate(IWithRelations objWithRelations, ManagementPackTypeProjection managementPackTypeProjection, Dictionary<string, object> properties, bool skipKeyProperties = false)
        {
            var entObj = objWithRelations.GetCoreEnterpriseManagementObject();
            var objectProperties = GetObjectPropertyDictionary(managementPackTypeProjection.TargetType);
            var normalizer = new ValueConverter(_client);

            foreach (var kv in properties)
            {
                var name = kv.Key;
                var value = kv.Value;

                if (objectProperties.TryGetValue(name, out var prop))
                {
                    if (skipKeyProperties && prop.Key)
                        continue;

                    if (value == null)
                        continue;

                    var val = normalizer.NormalizeValue(kv.Value, prop);
                    entObj[managementPackTypeProjection.TargetType, name].Value = val;

                    continue;
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

                    if (value?.GetType().IsEnumerableType() != true)
                    {
                        value = new List<object> { value };
                    }

                    var enu = value as IEnumerable;

                    var trel = managementPackTypeProjection.FirstOrDefault(p => p.Value.Alias == className);
                    if (trel.Value == null)
                    {
                        trel = managementPackTypeProjection.FirstOrDefault(p => p.Key.ParentElement.Name == className);
                    }

                    if (trel.Value == null)
                    {
                        throw new Exception(
                            $"Can't find a TypeProjection with Alias '{className}' or Relationship with Id '{className}'");
                    }

                    var relationShip = trel.Key.ParentElement as ManagementPackRelationship;

                    className = trel.Value.TargetType.Name;
                    

                    foreach (var o in enu)
                    {
                        var oVal = o;
                        var itemClassName = className;

                        if (!String.IsNullOrWhiteSpace(propertyName))
                        {
                            var foundobj = _cachedSearchedObjects.GetOrAdd($"{itemClassName}|{propertyName} -eq '{oVal}'", s =>
                            {
                                var splitted = s.Split("|".ToCharArray(), 2);
                                return GetEnterpriseManagementObjectsByClassName(splitted[0], splitted[1], new RetrievalOptions() { MaxResultCount = 1 }).FirstOrDefault();
                            });

                            //var foundobj = GetEnterpriseManagementObjectsByClassName(itemClassName, $"{propertyName} -eq '{oVal}'", new RetrievalOptions() {MaxResultCount = 1}).FirstOrDefault();
                            if (foundobj == null)
                            {
                                continue;
                            }
                            oVal = foundobj.Id;
                        }


                        if (oVal is JObject jObject)
                        {
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
                                    if (dict.ContainsKey("@class"))
                                    {
                                        itemClassName = dict["@class"].ToString();
                                    }


                                    var newRelation = BuildEnterpriseManagementObjectWithRelations(itemClassName, dict);
                                    newRelation.Relationship = relationShip;
                                    objWithRelations.AddRelatedObject(newRelation);
                                    break;
                                }
                            case Guid guid:
                                {

                                    var existingObject = _cachedObjects.GetOrAdd(guid, GetEnterpriseManagementObjectById);
                                    var related = new EnterpriseManagementObjectWithRelations(existingObject);
                                    related.Relationship = relationShip;
                                    objWithRelations.AddRelatedObject(related);
                                    break;
                                }
                        }

                    }

                    continue;
                }






            }
        }

        
    }

}
