using System.Collections.Generic;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.Mappers;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.ExtensionMethods
{
    public static class EnterpriseManagementObjectExtensions
    {
        //public static EnterpriseManagementObjectDto ToDto(this EnterpriseManagementObject enterpriseManagementObject)
        //{
        //    return ObjectMapper.ToDto(enterpriseManagementObject);
        //}

        public static EnterpriseManagementObjectProjectionDto ToObjectProjectionDto(this EnterpriseManagementObjectProjection composableProjection)
        {
            return ObjectMapper.ToObjectProjectionDto(composableProjection);
        }

        public static EnterpriseManagementObjectDto ToObjectDto(this EnterpriseManagementObject enterpriseManagementObject)
        {
            return ObjectMapper.ToObjectDto(enterpriseManagementObject);
        }
    }
}
