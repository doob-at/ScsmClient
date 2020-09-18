using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BaseIT.SCSM.Client.Model;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;

namespace BaseIT.SCSM.Client.Mappers
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
