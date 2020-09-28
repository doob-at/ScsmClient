using System.Collections.Generic;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.Mappers;
using ScsmClient.Model;

namespace ScsmClient.ExtensionMethods
{
    public static class EnterpriseManagementObjectExtensions
    {
        //public static EnterpriseManagementObjectDto ToDto(this EnterpriseManagementObject enterpriseManagementObject)
        //{
        //    return ObjectMapper.ToDto(enterpriseManagementObject);
        //}

        public static EnterpriseManagementObjectProjectionDto ToObjectProjectionDto(this IComposableProjection composableProjection)
        {
            return ObjectMapper.ToObjectProjectionDto(composableProjection);
        }

        public static EnterpriseManagementObjectDto ToObjectDto(this EnterpriseManagementObject enterpriseManagementObject)
        {
            return ObjectMapper.ToObjectDto(enterpriseManagementObject);
        }
    }
}
