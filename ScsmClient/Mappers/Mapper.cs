using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BaseIT.SCSM.Client.Model;
using Microsoft.EnterpriseManagement.Common;

namespace BaseIT.SCSM.Client.Mappers
{
    public static class ObjectMapper
    {
        static ObjectMapper()
        {
            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EnterpriseManagementObjectMapperProfile>();
            }).CreateMapper();
        }

        private static IMapper Mapper { get; }

        public static EnterpriseManagementObjectDto ToDto(EnterpriseManagementObject enterpriseManagementObject)
        {
            return enterpriseManagementObject == null
                ? null
                : Mapper.Map<EnterpriseManagementObjectDto>(enterpriseManagementObject);
        }
    }
}
