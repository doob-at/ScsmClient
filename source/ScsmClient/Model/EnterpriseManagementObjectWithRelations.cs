using System.Collections.Generic;
using Microsoft.EnterpriseManagement.Common;

namespace ScsmClient.Model
{
    public interface IWithRelations
    {
        void AddRelatedObject(EnterpriseManagementObjectWithRelations enterpriseManagementObject);

        List<EnterpriseManagementObjectWithRelations> RelatedObjects { get; set; }

        EnterpriseManagementObject GetCoreEnterpriseManagementObject();

    }

    public class EnterpriseManagementObjectWithRelations: IWithRelations
    {
        public EnterpriseManagementObject EnterpriseManagementObject { get; }

        public List<EnterpriseManagementObjectWithRelations> RelatedObjects { get; set; }

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

        public EnterpriseManagementObject GetCoreEnterpriseManagementObject()
        {
            return EnterpriseManagementObject;
        }
    }

    public class EnterpriseManagementObjectProjectionWithRelations: IWithRelations
    {
        public EnterpriseManagementObjectProjection EnterpriseManagementObjectProjection { get; }

        public List<EnterpriseManagementObjectWithRelations> RelatedObjects { get; set; }

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

        public EnterpriseManagementObject GetCoreEnterpriseManagementObject()
        {
            return EnterpriseManagementObjectProjection.Object;
        }
    }
}