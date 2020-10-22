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

        public ManagementPackRelationship FindRelationShip(string firstClassName, string secondClassName)
        {

            var firstClass = _client.Types().GetClassByName(firstClassName);
            var secondClass = _client.Types().GetClassByName(secondClassName);

            return FindRelationShip(firstClass, secondClass);
        }

        public ManagementPackRelationship FindRelationShip(ManagementPackClass firstClass, ManagementPackClass secondClass)
        {

            var relClasses = _client.ManagementGroup.EntityTypes.GetRelationshipClasses();

            //var firstClassTargets = relClasses.Where(r => r.Target.Type.GetElement().Id == firstClass.Id).ToList();
            //var firstClassSources = relClasses.Where(r => r.Source.Type.GetElement().Id == firstClass.Id).ToList();

            //var secondClassTargets = relClasses.Where(r => r.Target.Type.GetElement().Id == secondClass.Id).ToList();
            //var secondClassSources = relClasses.Where(r => r.Source.Type.GetElement().Id == secondClass.Id).ToList();


            var relationShipClass = relClasses.FirstOrDefault(r => 
                r.Source.Type.GetElement().Id == firstClass.Id && r.Target.Type.GetElement().Id == secondClass.Id ||
                r.Source.Type.GetElement().Id == secondClass.Id && r.Target.Type.GetElement().Id == firstClass.Id);

            if (relationShipClass == null)
            {
                secondClass = secondClass.Base?.GetElement();
                if (secondClass != null)
                {
                    return FindRelationShip(firstClass, secondClass);
                }
            }
            return relationShipClass;

        }

        public Guid AddRelatedObjectByRelationshipName(string relationshipName, Guid firstId, Guid secondId)
        {
            var relClass = _client.Types().GetRelationshipClassByName(relationshipName);
            var sourceObj = _client.ManagementGroup.EntityObjects.GetObject<EnterpriseManagementObject>(firstId, ObjectQueryOptions.Default);
            var targetObj = _client.ManagementGroup.EntityObjects.GetObject<EnterpriseManagementObject>(secondId, ObjectQueryOptions.Default);

            return AddRelatedObject(relClass, sourceObj, targetObj);
        }

        public Guid AddRelatedObject(ManagementPackRelationship relationship, EnterpriseManagementObject first, EnterpriseManagementObject second)
        {
            var relationshipObject = buildCreatableEnterpriseManagementRelationshipObject(relationship, first, second);
            
            relationshipObject.Commit();
            
            return relationshipObject.Id;
        }

        internal CreatableEnterpriseManagementRelationshipObject buildCreatableEnterpriseManagementRelationshipObject(EnterpriseManagementObject first, EnterpriseManagementObject second)
        {
            var relationshipClass = FindRelationShip(first.GetClasses(BaseClassTraversalDepth.None).First().Name,
                second.GetClasses(BaseClassTraversalDepth.None).First().Name);

            return buildCreatableEnterpriseManagementRelationshipObject(relationshipClass, first, second);
        }

        internal CreatableEnterpriseManagementRelationshipObject buildCreatableEnterpriseManagementRelationshipObject(ManagementPackRelationship relationship, EnterpriseManagementObject first, EnterpriseManagementObject second)
        {
            var targetClass = relationship.Target.Type.GetElement();
            var sourceClass = relationship.Source.Type.GetElement();

            //var firstObjectClass = first.GetClasses(BaseClassTraversalDepth.None).FirstOrDefault();
            //var secondObjectClass = second.GetClasses(BaseClassTraversalDepth.None).FirstOrDefault();

            var targetObject = first.IsInstanceOf(targetClass) ? first : (second.IsInstanceOf(targetClass) ? second : null);
            var sourceObject = second.IsInstanceOf(sourceClass) ? second : (first.IsInstanceOf(sourceClass) ? first : null);

            if (targetObject == null || sourceObject == null)
            {
                throw new Exception($"Types doesn't match, valid Types are '{targetClass.Name}' and '{sourceClass.Name}'");
            }


            var relationshipObject = new CreatableEnterpriseManagementRelationshipObject(_client.ManagementGroup, relationship);

            relationshipObject.SetSource(sourceObject);
            relationshipObject.SetTarget(targetObject);

            return relationshipObject;
        }


        public void RemoveRelationShip<T>(EnterpriseManagementRelationshipObject<T> rel) where T : EnterpriseManagementObject
        {

            var idd = new IncrementalDiscoveryData();
            idd.Remove(rel);
            idd.Commit(_client.ManagementGroup);

        }
        
    }
}
