using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;

namespace ScsmClient.Operations
{
    public class RelationsOperations: BaseOperation
    {
       
        public RelationsOperations(SCSMClient client) : base(client)
        {

            
        }



        public Guid AddRelatedObjectByRelationshipName(string relationshipName, Guid sourceId, Guid targetId)
        {
            var relClass = _client.Class().GetRelationshipClassByName(relationshipName);
            var sourceObj = _client.ManagementGroup.EntityObjects.GetObject<EnterpriseManagementObject>(sourceId, ObjectQueryOptions.Default);
            var targetObj = _client.ManagementGroup.EntityObjects.GetObject<EnterpriseManagementObject>(targetId, ObjectQueryOptions.Default);

            return AddRelatedObject(relClass, sourceObj, targetObj);
        }

        public Guid AddRelatedObject(ManagementPackRelationship relationship, EnterpriseManagementObject source, EnterpriseManagementObject target)
        {

            var relationshipObject = new CreatableEnterpriseManagementRelationshipObject(_client.ManagementGroup, relationship);
            
            relationshipObject.SetSource(source);
            relationshipObject.SetTarget(target);
            
            relationshipObject.Commit();
            
            return relationshipObject.Id;
        }

        
        public void RemoveRelationShip<T>(EnterpriseManagementRelationshipObject<T> rel) where T : EnterpriseManagementObject
        {

            var idd = new IncrementalDiscoveryData();
            idd.Remove(rel);
            idd.Commit(_client.ManagementGroup);

        }
        
       
    }
}
