using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using ScsmClient.ExtensionMethods;

namespace ScsmClient.Operations
{
    public class RelationsOperations : BaseOperation
    {

        public RelationsOperations(SCSMClient client) : base(client)
        {


        }



        public ManagementPackRelationship FindRelationship(string firstClassName, string secondClassName)
        {

            var firstClass = _client.Types().GetClassByName(firstClassName);
            var secondClass = _client.Types().GetClassByName(secondClassName);

            return FindRelationship(firstClass, secondClass);
        }

        public ManagementPackRelationship FindRelationship(ManagementPackClass sourceClass, ManagementPackClass targetClass)
        {

            var relClasses = _client.ManagementGroup.EntityTypes.GetRelationshipClasses();

            var relationShipClass = relClasses.FirstOrDefault(r =>
                r.Source.Type.GetElement().Id == sourceClass.Id && r.Target.Type.GetElement().Id == targetClass.Id ||
                r.Source.Type.GetElement().Id == targetClass.Id && r.Target.Type.GetElement().Id == sourceClass.Id);

            if (relationShipClass == null)
            {
                var _sourceClass = sourceClass;
                while (_sourceClass != null)
                {
                    var found = FindRelationshipForTargetBaseClasses(_sourceClass, targetClass, relClasses);
                    if (found != null)
                    {
                        return found;
                    }
                    _sourceClass = _sourceClass.Base?.GetElement();
                }
                throw new Exception($"Can't find a realtion between '{sourceClass.Name}' and '{targetClass.Name}'");
            }
            return relationShipClass;

        }

        private ManagementPackRelationship FindRelationshipForTargetBaseClasses(ManagementPackClass firstClass, ManagementPackClass secondClass, IList<ManagementPackRelationship> relationships)
        {


            var relationShipClass = relationships.FirstOrDefault(r =>
                r.Source.Type.GetElement().Id == firstClass.Id && r.Target.Type.GetElement().Id == secondClass.Id ||
                r.Source.Type.GetElement().Id == secondClass.Id && r.Target.Type.GetElement().Id == firstClass.Id);

            if (relationShipClass == null)
            {
                var _secondClass = secondClass.Base?.GetElement();
                if (_secondClass != null)
                {
                    return FindRelationshipForTargetBaseClasses(firstClass, _secondClass, relationships);
                }
            }
            return relationShipClass;

        }

        private ManagementPackRelationship FindRelationshipForSourceBaseClasses(ManagementPackClass firstClass, ManagementPackClass secondClass, IList<ManagementPackRelationship> relationships)
        {


            var relationShipClass = relationships.FirstOrDefault(r =>
                r.Source.Type.GetElement().Id == firstClass.Id && r.Target.Type.GetElement().Id == secondClass.Id ||
                r.Source.Type.GetElement().Id == secondClass.Id && r.Target.Type.GetElement().Id == firstClass.Id);

            if (relationShipClass == null)
            {
                var _firstClass = firstClass.Base?.GetElement();
                if (_firstClass != null)
                {
                    return FindRelationshipForSourceBaseClasses(_firstClass, secondClass, relationships);
                }
            }
            return relationShipClass;

        }


        public Guid CreateRelation(Guid sourceId, Guid targetId)
        {
            var sourceObject = _client.Object().GetEnterpriseManagementObjectById(sourceId);
            var targetObject = _client.Object().GetEnterpriseManagementObjectById(targetId);
            return CreateRelation(sourceObject, targetObject);
        }

        public object CreateRelation(Guid sourceId, EnterpriseManagementObject targetObject)
        {
            var sourceObject = _client.Object().GetEnterpriseManagementObjectById(sourceId);
            return CreateRelation(sourceObject, targetObject);
        }

        public Guid CreateRelation(EnterpriseManagementObject sourceObject, Guid targetId)
        {
            var targetObject = _client.Object().GetEnterpriseManagementObjectById(targetId);
            return CreateRelation(sourceObject, targetObject);
        }

        public Guid CreateRelation(EnterpriseManagementObject sourceObject, EnterpriseManagementObject targetObject)
        {
            var relationship = FindRelationship(sourceObject.GetManagementPackClass(),
                 targetObject.GetManagementPackClass());

            return CreateRelation(relationship, sourceObject, targetObject);
        }

        public Guid CreateRelationByName(string relationshipName, Guid sourceId, Guid targetId)
        {
            var relClass = _client.Types().GetRelationshipClassByName(relationshipName);
            var sourceObj = _client.Object().GetEnterpriseManagementObjectById(sourceId);
            var targetObj = _client.Object().GetEnterpriseManagementObjectById(targetId);

            return CreateRelation(relClass, sourceObj, targetObj);
        }

        public Guid CreateRelation(ManagementPackRelationship relationship, EnterpriseManagementObject first, EnterpriseManagementObject second)
        {
            var relationshipObject = buildCreatableEnterpriseManagementRelationshipObject(relationship, first, second);

            relationshipObject.Commit();

            return relationshipObject.Id;
        }



        public IEnumerable<EnterpriseManagementObject> GetAllRelatedObjects(Guid sourceId)
        {
            return GetRelatedObjectsByClassId(sourceId, Guid.Empty);
        }
        public IEnumerable<EnterpriseManagementObject> GetRelatedObjectsByClassName(Guid sourceId, string className)
        {
            var managementPackClass = _client.Types().GetClassByName(className);
            return GetRelatedObjectsByClassId(sourceId, managementPackClass.Id);
        }
        public IEnumerable<EnterpriseManagementObject> GetRelatedObjectsByClass(Guid sourceId, ManagementPackClass managementPackClass)
        {
            return GetRelatedObjectsByClassId(sourceId, managementPackClass.Id);
        }
        public IEnumerable<EnterpriseManagementObject> GetRelatedObjectsByClassId(Guid sourceId, Guid managementPackClassId)
        {
            return GetRelationshipObjectsByClassId(sourceId, managementPackClassId).Select(ro => ro.TargetObject);
        }

        public IEnumerable<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> GetAllRelationshipObjects(Guid sourceId)
        {
            return GetRelationshipObjectsByClassId(sourceId, Guid.Empty);
        }
        public IEnumerable<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> GetRelationshipObjectsByClassName(Guid sourceId, string className)
        {
            var managementPackClass = _client.Types().GetClassByName(className);
            return GetRelationshipObjectsByClassId(sourceId, managementPackClass.Id);
        }

        public IEnumerable<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> GetRelationshipObjectsByClass(Guid sourceId,
            ManagementPackClass managementPackClass)
        {
            return GetRelationshipObjectsByClassId(sourceId, managementPackClass.Id);
        }
        public IEnumerable<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> GetRelationshipObjectsByClassId(Guid sourceId, Guid managementPackClassId)
        {
            var query = _client.ManagementGroup.EntityObjects
                .GetRelationshipObjects<EnterpriseManagementObject>(sourceId, ObjectQueryOptions.Default)
                .OrderBy(ro => ro.LastModified).ToList();

            if (managementPackClassId != Guid.Empty)
            {
                query = query.Where(ro =>
                {
                    var isTargetClass = ro.TargetObject.GetManagementPackClass().Id == managementPackClassId;
                    var isSourceClass = ro.SourceObject.GetManagementPackClass().Id == managementPackClassId;
                    return isSourceClass || isTargetClass;
                }).ToList();
            }

            return query;
        }



        internal CreatableEnterpriseManagementRelationshipObject buildCreatableEnterpriseManagementRelationshipObject(EnterpriseManagementObject first, EnterpriseManagementObject second)
        {
            var relationshipClass = FindRelationship(first.GetManagementPackClass(), second.GetManagementPackClass());
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


        internal void RemoveRelation<T>(EnterpriseManagementRelationshipObject<T> rel) where T : EnterpriseManagementObject
        {

            var idd = new IncrementalDiscoveryData();
            idd.Remove(rel);
            idd.Commit(_client.ManagementGroup);

        }


    }
}
