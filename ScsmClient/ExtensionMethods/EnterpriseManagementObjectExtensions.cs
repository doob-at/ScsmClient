using Microsoft.EnterpriseManagement.Common;
using ScsmClient.Mappers;
using ScsmClient.Model;

namespace ScsmClient.ExtensionMethods
{
    public static class EnterpriseManagementObjectExtensions
    {
        public static EnterpriseManagementObjectDto ToDto(this EnterpriseManagementObject enterpriseManagementObject)
        {
            return ObjectMapper.ToDto(enterpriseManagementObject);
        }
    }
}
