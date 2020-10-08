using AutoMapper;
using Microsoft.EnterpriseManagement.Common;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Mappers
{
    internal static class ObjectMapper
    {
        private static IMapper EnterpriseManagementObjectMapper { get; }
        private static IMapper EnterpriseManagementObjectProjectionMapper { get; }

        static ObjectMapper()
        {
            EnterpriseManagementObjectMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EnterpriseManagementObjectMapperProfile>();
            }).CreateMapper();

            EnterpriseManagementObjectProjectionMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EnterpriseManagementObjectProjectionMapperProfile>();
            }).CreateMapper();
        }

        

        public static EnterpriseManagementObjectDto ToObjectDto(EnterpriseManagementObject enterpriseManagementObject)
        {
            return enterpriseManagementObject == null
                ? null
                : EnterpriseManagementObjectMapper.Map<EnterpriseManagementObjectDto>(enterpriseManagementObject);
        }

        public static EnterpriseManagementObjectProjectionDto ToObjectProjectionDto(IComposableProjection composableProjection)
        {
            return composableProjection == null
                ? null
                : EnterpriseManagementObjectProjectionMapper.Map<EnterpriseManagementObjectProjectionDto>(composableProjection);
        }
    }
}
