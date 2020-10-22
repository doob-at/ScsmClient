using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using ScsmClient.ExtensionMethods;
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


        public IEnumerable<EnterpriseManagementObjectProjection> GetTypeProjectionObjects(Guid typeProjectionGuid, string criteria, int? maxResult = null, int? levels = null)
        {
            var tp = _client.Types().GetTypeProjectionById(typeProjectionGuid);
            return GetTypeProjectionObjects(tp, criteria, maxResult, levels);
        }


        public IEnumerable<EnterpriseManagementObjectProjection> GetTypeProjectionObjects(string typeProjectionName, string criteria, int? maxResult = null, int? levels = null)
        {
            var tp = _client.Types().GetTypeProjectionByName(typeProjectionName);
            return GetTypeProjectionObjects(tp, criteria, maxResult, levels);
        }

        public IEnumerable<EnterpriseManagementObjectProjection> GetTypeProjectionObjects(ManagementPackTypeProjection typeProjection,
            string criteria, int? maxResult = null, int? levels = null)
        {
            
            var crit = _client.Criteria().BuildObjectProjectionCriteria(criteria, typeProjection);

            var critOptions = new ObjectQueryOptions();

            critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            if (maxResult.HasValue && maxResult.Value != int.MaxValue)
            {
                critOptions.MaxResultCount = maxResult.Value;
                critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.Buffered;
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
