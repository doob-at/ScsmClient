using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using doob.Reflectensions.Common.Helper;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using ScsmClient.ExtensionMethods;
using ScsmClient.Model;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Operations
{
    public class TypeProjectionOperations: BaseOperation
    {
        internal TypeProjectionOperations(SCSMClient client) : base(client)
        {
        }


        private IObjectProjectionReader<EnterpriseManagementObject> GetObjectProjectionReader(ObjectProjectionCriteria criteria, ObjectQueryOptions options)
        {
            return _client.ManagementGroup.EntityObjects.GetObjectProjectionReader<EnterpriseManagementObject>(criteria, options);
        }


        public IEnumerable<EnterpriseManagementObjectProjection> GetTypeProjectionObjects(Guid typeProjectionGuid, string criteria, RetrievalOptions retrievalOptions = null)
        {
            var tp = _client.Types().GetTypeProjectionById(typeProjectionGuid);
            return GetTypeProjectionObjects(tp, criteria, retrievalOptions);
        }


        public IEnumerable<EnterpriseManagementObjectProjection> GetTypeProjectionObjects(string typeProjectionName, string criteria, RetrievalOptions retrievalOptions = null)
        {
            var tp = _client.Types().GetTypeProjectionByName(typeProjectionName);
            return GetTypeProjectionObjects(tp, criteria, retrievalOptions);
        }

        public IEnumerable<EnterpriseManagementObjectProjection> GetTypeProjectionObjects(ManagementPackTypeProjection typeProjection,
            string criteria, RetrievalOptions retrievalOptions = null)
        {
            
            var crit = _client.Criteria().BuildObjectProjectionCriteria(criteria, typeProjection);

            var critOptions = new ObjectQueryOptions();
            critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.Buffered;

            retrievalOptions = retrievalOptions ?? new RetrievalOptions();

            if (retrievalOptions.PropertiesToLoad != null)
            {
                critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.None;
                if (retrievalOptions.PropertiesToLoad.Any())
                {
                    var objectClassProperties = GetAllProperties(typeProjection, null);
                    foreach (var s in retrievalOptions.PropertiesToLoad)
                    {

                        var propsToLoad = objectClassProperties.Where(op => WildcardHelper.Match(op.FullName, s));

                        foreach (var propToLoad in propsToLoad)
                        {
                            if (propToLoad != null)
                            {
                                critOptions.AddPropertyToRetrieve(propToLoad.TargetType, propToLoad.Property);

                            }
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
            //var sortprop = new EnterpriseManagementObjectGenericProperty(EnterpriseManagementObjectGenericPropertyName.TimeAdded);
            var sortCrit = _client.Criteria().CreateSortCriteriaXmlFrom("-G:TimeAdded", typeProjection);
            critOptions.AddSortProperty(sortCrit, typeProjection, _client.ManagementGroup);

            var reader = GetObjectProjectionReader(crit, critOptions);
           
            foreach (var enterpriseManagementObjectProjection in reader)
            {
                yield return enterpriseManagementObjectProjection;
            }

        }


        private List<PropertyMap> GetAllProperties(ITypeProjectionComponent managementPackTypeProjection, ManagementPackClass parentClass)
        {
            var properties = GetAllProperties(managementPackTypeProjection.TargetType, parentClass);
            var nextParentClass = managementPackTypeProjection.TargetType;

            foreach (var keyValuePair in managementPackTypeProjection)
            {
                var props = GetAllProperties(keyValuePair.Value, nextParentClass);
                properties.AddRange(props);
            }

            return properties;
        }


        private List<PropertyMap> GetAllProperties(ManagementPackClass managementPackClass, ManagementPackClass parentClass)
        {

            if (managementPackClass.Abstract)
            {
                return new List<PropertyMap>();
            }
            return managementPackClass
                .GetProperties(BaseClassTraversalDepth.Recursive)
                .Select(p =>
                {
                    var pm = new PropertyMap();
                    if (parentClass != null)
                    {
                        pm.FullName = $"{managementPackClass.Name}!{p.Name}";
                    }
                    else
                    {
                        pm.FullName = p.Name;
                    }

                    pm.TargetType = managementPackClass;
                    pm.Property = p;
                    return pm;
                }).ToList();

        }

    }

    internal class PropertyMap
    {
        public string FullName { get; set; }

        public ManagementPackProperty Property { get; set; }

        public ManagementPackClass TargetType { get; set; }

    }
}
