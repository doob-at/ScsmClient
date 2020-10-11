using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.SharedModels.Models;

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

                        var dict = new Dictionary<string, object>();
                        foreach (var enterpriseManagementSimpleObject in src.Values)
                        {
                            var value = enterpriseManagementSimpleObject.Value;
                            if (value is ManagementPackEnumeration en)
                            {
                                value = en.DisplayName;
                            }

                            if (!dict.ContainsKey(enterpriseManagementSimpleObject.Type.Name))
                            {
                                dict.Add(enterpriseManagementSimpleObject.Type.Name, value);
                            }
                        }
                        return dict;
                    });
                });

        }
    }

    internal class EnterpriseManagementObjectProjectionMapperProfile : Profile
    {

        public EnterpriseManagementObjectProjectionMapperProfile()
        {

            CreateMap<EnterpriseManagementObject, EnterpriseManagementObjectProjectionDto>()
                 .ForMember(dto => dto.Values, expression =>
                 {
                     expression.MapFrom((src, dto) =>
                     {

                         var dict = new Dictionary<string, object>();
                         foreach (var enterpriseManagementSimpleObject in src.Values)
                         {
                             var value = enterpriseManagementSimpleObject.Value;
                             if (value is ManagementPackEnumeration en)
                             {
                                 value = en.DisplayName;
                             }
                             if (!dict.ContainsKey(enterpriseManagementSimpleObject.Type.Name))
                             {
                                 dict.Add(enterpriseManagementSimpleObject.Type.Name, value);
                             }
                         }
                         return dict;
                     });
                 });


            CreateMap<IComposableProjection, EnterpriseManagementObjectProjectionDto>()

                .ForMember(dto => dto.Values, expression =>
                {
                    expression.MapFrom((src, dto) =>
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (var enterpriseManagementSimpleObject in src.Object.Values)
                        {
                            var value = enterpriseManagementSimpleObject.Value;
                            if (value is ManagementPackEnumeration en)
                            {
                                value = en.DisplayName;
                            }
                            if (!dict.ContainsKey(enterpriseManagementSimpleObject.Type.Name))
                            {
                                dict.Add(enterpriseManagementSimpleObject.Type.Name, value);
                            }
                        }
                        return dict;
                    });
                })
                .ForMember(dto => dto.RelatedObjects, expression =>
                {
                    expression.MapFrom((src, dto) =>
                    {
                        Dictionary<string, RelatedObjects> related = new Dictionary<string, RelatedObjects>();

                        foreach (var keyValuePair in src)
                        {
                            var relationShip = new RelationShip();
                            relationShip.Name = keyValuePair.Key.ParentElement.Name;

                            if (!related.ContainsKey(keyValuePair.Key.Name))
                            {
                                related.Add(keyValuePair.Key.Name, new RelatedObjects());
                            }

                            related[keyValuePair.Key.Name].RelationShip = relationShip;
                            related[keyValuePair.Key.Name].Objects.Add(ObjectMapper.ToObjectProjectionDto(keyValuePair.Value));
                        }

                        return related;
                    });
                })
                .IncludeMembers(o => o, s => s.Object);



        }
    }
}
