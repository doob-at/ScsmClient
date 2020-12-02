using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            if (retrievalOptions.PropertiesToLoad != null)
            {
                critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.None;
                if (retrievalOptions.PropertiesToLoad.Any())
                {
                    var objectClassProperties = typeProjection.TargetType.GetProperties(BaseClassTraversalDepth.Recursive);
                    foreach (var s in retrievalOptions.PropertiesToLoad)
                    {
                        var prop = objectClassProperties.FirstOrDefault(ocp => ocp.Name.Equals(s));
                        if (prop != null)
                        {
                            critOptions.AddPropertyToRetrieve(typeProjection.TargetType, prop);

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


        

       
    }
}
