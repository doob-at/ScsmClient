using System.Linq;
using AutoMapper;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.Model;

namespace ScsmClient.Mappers
{
    public class EnterpriseManagementObjectMapperProfile : Profile
    {

        public EnterpriseManagementObjectMapperProfile()
        {
            CreateMap<EnterpriseManagementObject, EnterpriseManagementObjectDto>()
                .ForMember(dto => dto.Values, expression =>
                {
                    expression.MapFrom((src, dto) =>
                    {
                        return src.Values.ToDictionary(
                            v => v.Type.Name,
                            v =>
                            {
                                if (v.Value is ManagementPackEnumeration en)
                                {
                                    return en.DisplayName;
                                }

                                return v.Value;
                            });
                    });
                });
        }
    }
}
