using System.Collections.Generic;
using Microsoft.EnterpriseManagement.Common;

namespace ScsmClient.Model
{
    //public class UpdateEnterpriseManagementObjectWithRelations
    //{
    //    public EnterpriseManagementObject EnterpriseManagementObject { get; }

    //    public List<UpdateEnterpriseManagementObjectWithRelations> RelatedObjects { get; set; }

    //    public UpdateEnterpriseManagementObjectWithRelations(EnterpriseManagementObject enterpriseManagementObject)
    //    {
    //        EnterpriseManagementObject = enterpriseManagementObject;
    //    }

    //    public UpdateEnterpriseManagementObjectWithRelations AddRelatedObject(
    //        UpdateEnterpriseManagementObjectWithRelations creatableEnterpriseManagementObject)
    //    {
    //        if (RelatedObjects == null)
    //        {
    //            RelatedObjects = new List<UpdateEnterpriseManagementObjectWithRelations>();
    //        }
    //        RelatedObjects.Add(creatableEnterpriseManagementObject);
    //        return this;
    //    }
    //}

    public class CreatableEnterpriseManagementObjectWithRelations
    {
        public EnterpriseManagementObject CreatableEnterpriseManagementObject { get; }

        public List<CreatableEnterpriseManagementObjectWithRelations> RelatedObjects { get; set; }

        public CreatableEnterpriseManagementObjectWithRelations(EnterpriseManagementObject creatableEnterpriseManagementObject)
        {
            CreatableEnterpriseManagementObject = creatableEnterpriseManagementObject;
        }

        public CreatableEnterpriseManagementObjectWithRelations AddRelatedObject(
            CreatableEnterpriseManagementObjectWithRelations creatableEnterpriseManagementObject)
        {
            if (RelatedObjects == null)
            {
                RelatedObjects = new List<CreatableEnterpriseManagementObjectWithRelations>();
            }
            RelatedObjects.Add(creatableEnterpriseManagementObject);
            return this;
        }
    }
}