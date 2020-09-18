using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseIT.SCSM.Client.Mappers;
using BaseIT.SCSM.Client.Model;
using Microsoft.EnterpriseManagement.Common;

namespace BaseIT.SCSM.Client.ExtensionMethods
{
    public static class EnterpriseManagementObjectExtensions
    {
        public static EnterpriseManagementObjectDto ToDto(this EnterpriseManagementObject enterpriseManagementObject)
        {
            return ObjectMapper.ToDto(enterpriseManagementObject);
        }
    }
}
