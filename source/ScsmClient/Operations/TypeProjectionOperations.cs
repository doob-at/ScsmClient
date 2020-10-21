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


        //public IObjectProjectionReader<EnterpriseManagementObject> GetObjectProjectionReader(ObjectProjectionCriteria criteria)
        //{
        //    return _client.ManagementGroup.EntityObjects.GetObjectProjectionReader<EnterpriseManagementObject>(criteria, ObjectQueryOptions.Default);
        //}

        private IObjectProjectionReader<EnterpriseManagementObject> GetObjectProjectionReader(ObjectProjectionCriteria criteria, ObjectQueryOptions options)
        {
            return _client.ManagementGroup.EntityObjects.GetObjectProjectionReader<EnterpriseManagementObject>(criteria, options);
        }

        public ManagementPackTypeProjection GetTypeProjectionById(Guid typeProjectionId)
        {
            return _client.ManagementGroup.EntityTypes.GetTypeProjection(typeProjectionId);
        }

        public ManagementPackTypeProjection GetTypeProjectionByClassName(string className, ManagementPack managementPack)
        {
            return _client.ManagementGroup.EntityTypes.GetTypeProjection(className, managementPack);
        }

        public ManagementPackTypeProjection GetTypeProjectionByClassName(string className)
        {
            var crit = new ManagementPackTypeProjectionCriteria($"Name='{className}'");
            return GetTypeProjectionsByCriteria(crit).FirstOrDefault();
        }

        //public ManagementPackTypeProjection GetTypeProjection(string typeProjectionId)
        //{
        //    return GetTypeProjectionById(new Guid(typeProjectionId));
        //}

        public IList<ManagementPackTypeProjection> GetTypeProjectionsByCriteria(ManagementPackTypeProjectionCriteria criteria = null)
        {
            return criteria == null ? _client.ManagementGroup.EntityTypes.GetTypeProjections() :_client.ManagementGroup.EntityTypes.GetTypeProjections(criteria);
        }

        public IList<ManagementPackTypeProjection> GetTypeProjectionsByCriteria(string criteria)
        {
            var crit = new ManagementPackTypeProjectionCriteria(criteria);
            return _client.ManagementGroup.EntityTypes.GetTypeProjections(crit);
        }


        public IEnumerable<ScsmObject> GetObjectProjectionObjects(Guid typeProjectionGuid, string criteria, int? maxResult = null, int? levels = null)
        {
            var tp = GetTypeProjectionById(typeProjectionGuid);
            return GetObjectProjectionObjects(tp, criteria, maxResult, levels);
        }


        public IEnumerable<ScsmObject> GetObjectProjectionObjects(string typeProjectionName, string criteria, int? maxResult = null, int? levels = null)
        {
            var tp = GetTypeProjectionByClassName(typeProjectionName);
            return GetObjectProjectionObjects(tp, criteria, maxResult, levels);
        }

        public IEnumerable<ScsmObject> GetObjectProjectionObjects(ManagementPackTypeProjection typeProjection,
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
                yield return enterpriseManagementObjectProjection.ToScsmObject(levels);
            }

        }
    }
}
