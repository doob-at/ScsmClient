using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.ExtensionMethods;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Operations
{
    public class TypeProjectionOperations: BaseOperation
    {
        internal TypeProjectionOperations(SCSMClient client) : base(client)
        {
        }


        public IObjectProjectionReader<EnterpriseManagementObject> GetObjectProjectionReader(ObjectProjectionCriteria criteria)
        {
            return _client.ManagementGroup.EntityObjects.GetObjectProjectionReader<EnterpriseManagementObject>(criteria, ObjectQueryOptions.Default);
        }

        public IObjectProjectionReader<EnterpriseManagementObject> GetObjectProjectionReader(ObjectProjectionCriteria criteria, ObjectQueryOptions options)
        {
            return _client.ManagementGroup.EntityObjects.GetObjectProjectionReader<EnterpriseManagementObject>(criteria, options);
        }

        public ManagementPackTypeProjection GetTypeProjection(Guid typeProjectionId)
        {
            return _client.ManagementGroup.EntityTypes.GetTypeProjection(typeProjectionId);
        }

        public ManagementPackTypeProjection GetTypeProjection(string className, ManagementPack managementPack)
        {
            return _client.ManagementGroup.EntityTypes.GetTypeProjection(className, managementPack);
        }

        public ManagementPackTypeProjection GetTypeProjectionByClassName(string className)
        {
            return GetTypeProjections(_client.Criteria()
                .BuildManagementPackTypeProjectionCriteria($"Name='{className}'")).FirstOrDefault();
        }

        public ManagementPackTypeProjection GetTypeProjection(string typeProjectionId)
        {
            return GetTypeProjection(new Guid(typeProjectionId));
        }

        public IList<ManagementPackTypeProjection> GetTypeProjections(ManagementPackTypeProjectionCriteria criteria = null)
        {
            return criteria == null ? _client.ManagementGroup.EntityTypes.GetTypeProjections() :_client.ManagementGroup.EntityTypes.GetTypeProjections(criteria);
        }

        public IList<ManagementPackTypeProjection> GetTypeProjections(string criteria)
        {
            var crit = _client.Criteria().BuildManagementPackTypeProjectionCriteria(criteria);
            return _client.ManagementGroup.EntityTypes.GetTypeProjections(crit);
        }



        public IEnumerable<EnterpriseManagementObjectProjectionDto> GetObjectProjectionObjects(string typeProjectionName,
            string criteria, int? maxResult = null, CancellationToken cancellationToken = default)
        {
            var tp = GetTypeProjectionByClassName(typeProjectionName);
            var crit = _client.Criteria().BuildObjectProjectionCriteria(criteria, tp);

            var critOptions = new ObjectQueryOptions();
            critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.NonBuffered;
            critOptions.MaxResultCount = maxResult ?? Int32.MaxValue;
            
            
            
            
            var reader = GetObjectProjectionReader(crit, critOptions);
            var count = 0;
            foreach (var enterpriseManagementObjectProjection in reader)
            {
                if (count == critOptions.MaxResultCount)
                    break;
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                yield return enterpriseManagementObjectProjection.ToObjectProjectionDto();
            }

        }
    }
}
