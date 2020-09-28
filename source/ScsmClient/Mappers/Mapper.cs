using AutoMapper;
using Microsoft.EnterpriseManagement.Common;
using ScsmClient.Model;

namespace ScsmClient.Mappers
{
    internal static class ObjectMapper
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

        public static EnterpriseManagementObjectProjectionDto ToDto(IComposableProjection composableProjection)
        {
            return composableProjection == null
                ? null
                : Mapper.Map<EnterpriseManagementObjectProjectionDto>(composableProjection);
        }
    }
}
