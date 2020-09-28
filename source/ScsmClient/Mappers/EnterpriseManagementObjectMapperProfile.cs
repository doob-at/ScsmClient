using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.Model;

namespace ScsmClient.Mappers
{
    internal class EnterpriseManagementObjectMapperProfile : Profile
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


            CreateMap<IComposableProjection, EnterpriseManagementObjectProjectionDto>()
                
                .ForMember(dto => dto.Values, expression =>
                {
                    expression.MapFrom((src, dto) =>
                    {
                        return src.Object.Values.ToDictionary(
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
                })
                .ForMember(dto => dto.RelatedObjects, expression =>
                {
                    expression.MapFrom((src, dto) =>
                    {
                        Dictionary<string, List<IComposableProjection>> related = new Dictionary<string, List<IComposableProjection>>();
                        foreach (var keyValuePair in src)
                        {
                            if (!related.ContainsKey(keyValuePair.Key.Name))
                            {
                                related.Add(keyValuePair.Key.Name, new List<IComposableProjection>());
                            }

                            related[keyValuePair.Key.Name].Add(keyValuePair.Value);
                        }

                        return related;
                    });
                })
                .IncludeMembers(o=> o, s => s.Object);

        }
    }
}
