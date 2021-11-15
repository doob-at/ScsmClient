using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using ScsmClient.Caches;
using ScsmClient.ExtensionMethods;
using ScsmClient.Helper;
using ScsmClient.Model;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Operations
{
    public class ScsmObjectOperations: BaseOperation
    {
       
        public ScsmObjectOperations(SCSMClient client) : base(client)
        {
        }


        #region Query
        public ScsmObject GetObjectById(Guid id)
        {
            return _client.Object().GetEnterpriseManagementObjectById(id).ToScsmObject();
        }

        public IEnumerable<ScsmObject> GetObjectsByTypeName(string typeName, string criteria, RetrievalOptions retrievalOptions = null)
        {
            var objectClass = _client.Types().GetClassByName(typeName);
            if (objectClass != null)
            {
                return GetObjectsByType(objectClass, criteria, retrievalOptions);
            }

            var typeProjectionClass = _client.Types().GetTypeProjectionByName(typeName);
            if (typeProjectionClass != null)
            {
                return GetObjectsByType(typeProjectionClass, criteria, retrievalOptions);
            }

            throw new ObjectNotFoundException($"Can't find a ManagementPackClass nor a TypeProjection with the name '{typeName}'");
        }

        public IEnumerable<ScsmObject> GetObjectsByTypeId(Guid classId, string criteria, RetrievalOptions retrievalOptions = null)
        {
            var objectClass = _client.Types().GetClassById(classId);
            if (objectClass != null)
            {
                return GetObjectsByType(objectClass, criteria, retrievalOptions);
            }

            var typeProjectionClass = _client.Types().GetTypeProjectionById(classId);
            if (typeProjectionClass != null)
            {
                return GetObjectsByType(typeProjectionClass, criteria, retrievalOptions);
            }

            throw new ObjectNotFoundException($"Can't find a ManagementPackClass nor a TypeProjection with the id '{classId}'");
        }

        public IEnumerable<ScsmObject> GetObjectsByType(ManagementPackClass objectClass, string criteria, RetrievalOptions retrievalOptions = null)
        {
            return _client.Object().GetEnterpriseManagementObjectsByClass(objectClass, criteria, retrievalOptions)
                .Select(obj => obj.ToScsmObject());
        }

        public IEnumerable<ScsmObject> GetObjectsByType(ManagementPackTypeProjection typeProjection, string criteria, RetrievalOptions retrievalOptions = null)
        {
            return _client.TypeProjection().GetTypeProjectionObjects(typeProjection, criteria, retrievalOptions).Select(obj =>
            {
                return obj.ToScsmObject(typeProjection, retrievalOptions?.ReferenceLevels);
            });

        }
        #endregion

        


    }
}
