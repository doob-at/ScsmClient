using System;
using System.Collections.Generic;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;

namespace ScsmClient.Model
{
    public interface IWithRelations
    {
        void AddRelatedObject(EnterpriseManagementObjectWithRelations enterpriseManagementObject);

        void RemoveRelatedObject(EnterpriseManagementObject enterpriseManagementObject);
        void RemoveRelationship(EnterpriseManagementRelationshipObject<EnterpriseManagementObject> relationship);

        List<EnterpriseManagementObjectWithRelations> RelatedObjects { get; set; }

        EnterpriseManagementObject GetCoreEnterpriseManagementObject();

    }

    public class EnterpriseManagementObjectWithRelations: IWithRelations
    {
        public EnterpriseManagementObject EnterpriseManagementObject { get; }

        public List<EnterpriseManagementObjectWithRelations> RelatedObjects { get; set; } = new List<EnterpriseManagementObjectWithRelations>();
        public List<EnterpriseManagementObject> RemoveRelatedObjects { get; set; } = new List<EnterpriseManagementObject>();
        public List<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> RemoveRelationShip { get; set; } = new List<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>>();
        public ManagementPackRelationship Relationship { get; set; }

        public EnterpriseManagementObjectWithRelations(EnterpriseManagementObject enterpriseManagementObject)
        {
            EnterpriseManagementObject = enterpriseManagementObject;
        }

        public void AddRelatedObject(EnterpriseManagementObjectWithRelations enterpriseManagementObject)
        {
            RelatedObjects.Add(enterpriseManagementObject);
        }

        public void RemoveRelatedObject(EnterpriseManagementObject enterpriseManagementObject)
        {
            if (!RemoveRelatedObjects.Contains(enterpriseManagementObject))
            {
                RemoveRelatedObjects.Add(enterpriseManagementObject);
            }
        }

        public void RemoveRelationship(EnterpriseManagementRelationshipObject<EnterpriseManagementObject> relationship)
        {
            if (!RemoveRelationShip.Contains(relationship))
            {
                RemoveRelationShip.Add(relationship);
            }

        }

        public EnterpriseManagementObject GetCoreEnterpriseManagementObject()
        {
            return EnterpriseManagementObject;
        }
    }

    public class EnterpriseManagementObjectProjectionWithRelations: IWithRelations
    {
        public EnterpriseManagementObjectProjection EnterpriseManagementObjectProjection { get; }

        

        public List<EnterpriseManagementObjectWithRelations> RelatedObjects { get; set; } = new List<EnterpriseManagementObjectWithRelations>();
        public List<EnterpriseManagementObjectBaseWithProperties> RemoveRelatedObjects { get; set; } = new List<EnterpriseManagementObjectBaseWithProperties>();
        public List<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> RemoveRelationShip { get; set; } = new List<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>>();


        public EnterpriseManagementObjectProjectionWithRelations(EnterpriseManagementObjectProjection enterpriseManagementObjectProjection)
        {
            EnterpriseManagementObjectProjection = enterpriseManagementObjectProjection;
        }

        public void AddRelatedObject(EnterpriseManagementObjectWithRelations enterpriseManagementObject)
        {
            RelatedObjects.Add(enterpriseManagementObject);
        }

        public void RemoveRelatedObject(EnterpriseManagementObject objectGuid)
        {
            if (!RemoveRelatedObjects.Contains(objectGuid))
            {
                RemoveRelatedObjects.Add(objectGuid);
            }

        }

        public void RemoveRelationship(EnterpriseManagementRelationshipObject<EnterpriseManagementObject> relationship)
        {
            if (!RemoveRelationShip.Contains(relationship))
            {
                RemoveRelationShip.Add(relationship);
            }

        }

        public EnterpriseManagementObject GetCoreEnterpriseManagementObject()
        {
            return EnterpriseManagementObjectProjection.Object;
        }
    }
}