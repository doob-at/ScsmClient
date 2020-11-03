using System;
using System.Collections.Generic;
using Microsoft.EnterpriseManagement.Common;

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

        public List<EnterpriseManagementObjectWithRelations> RelatedObjects { get; set; }
        public List<EnterpriseManagementObject> RemoveRelatedObjects { get; set; }
        public List<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> RemoveRelationShip { get; set; }

        public EnterpriseManagementObjectWithRelations(EnterpriseManagementObject enterpriseManagementObject)
        {
            EnterpriseManagementObject = enterpriseManagementObject;
        }

        public void AddRelatedObject(EnterpriseManagementObjectWithRelations enterpriseManagementObject)
        {
            if (RelatedObjects == null)
            {
                RelatedObjects = new List<EnterpriseManagementObjectWithRelations>();
            }
            RelatedObjects.Add(enterpriseManagementObject);
            
        }

        public void RemoveRelatedObject(EnterpriseManagementObject enterpriseManagementObject)
        {
            if (RemoveRelatedObjects == null)
            {
                RemoveRelatedObjects = new List<EnterpriseManagementObject>();
            }

            if (!RemoveRelatedObjects.Contains(enterpriseManagementObject))
            {
                RemoveRelatedObjects.Add(enterpriseManagementObject);
            }

        }

        public void RemoveRelationship(EnterpriseManagementRelationshipObject<EnterpriseManagementObject> relationship)
        {
            if (RemoveRelationShip == null)
            {
                RemoveRelationShip = new List<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>>();
            }

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

        

        public List<EnterpriseManagementObjectWithRelations> RelatedObjects { get; set; }
        public List<EnterpriseManagementObjectBaseWithProperties> RemoveRelatedObjects { get; set; }
        public List<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> RemoveRelationShip { get; set; }


        public EnterpriseManagementObjectProjectionWithRelations(EnterpriseManagementObjectProjection enterpriseManagementObjectProjection)
        {
            EnterpriseManagementObjectProjection = enterpriseManagementObjectProjection;
        }

        public void AddRelatedObject(EnterpriseManagementObjectWithRelations enterpriseManagementObject)
        {
            if (RelatedObjects == null)
            {
                RelatedObjects = new List<EnterpriseManagementObjectWithRelations>();
            }
            RelatedObjects.Add(enterpriseManagementObject);
        }

        public void RemoveRelatedObject(EnterpriseManagementObject objectGuid)
        {
            if (RemoveRelatedObjects == null)
            {
                RemoveRelatedObjects = new List<EnterpriseManagementObjectBaseWithProperties>();
            }

            if (!RemoveRelatedObjects.Contains(objectGuid))
            {
                RemoveRelatedObjects.Add(objectGuid);
            }

        }

        public void RemoveRelationship(EnterpriseManagementRelationshipObject<EnterpriseManagementObject> relationship)
        {
            if (RemoveRelationShip == null)
            {
                RemoveRelationShip = new List<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>>();
            }

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