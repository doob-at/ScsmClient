//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.EnterpriseManagement.Common;
//using Microsoft.EnterpriseManagement.Configuration;
//using ScsmClient.SharedModels.Models;

//namespace ScsmClient.Mappers
//{
//    internal static class ObjectMapper
//    {
//        //private static IMapper EnterpriseManagementObjectMapper { get; }
//        //private static IMapper EnterpriseManagementObjectProjectionMapper { get; }

//        static ObjectMapper()
//        {
//            //EnterpriseManagementObjectMapper = new MapperConfiguration(cfg =>
//            //{
//            //    cfg.AddProfile<EnterpriseManagementObjectMapperProfile>();
//            //}).CreateMapper();

//            //EnterpriseManagementObjectProjectionMapper = new MapperConfiguration(cfg =>
//            //{
//            //    cfg.AddProfile<EnterpriseManagementObjectProjectionMapperProfile>();
//            //}).CreateMapper();
//        }

        

//        public static EnterpriseManagementObjectDto ToObjectDto(EnterpriseManagementObject enterpriseManagementObject)
//        {

//            var dto = new EnterpriseManagementObjectDto
//            {
//                Id = enterpriseManagementObject.Id,
//                Name = enterpriseManagementObject.Name,
//                Path = enterpriseManagementObject.Path,
//                DisplayName = enterpriseManagementObject.DisplayName,
//                FullName = enterpriseManagementObject.FullName,
//                ManagementPackClassIds = enterpriseManagementObject.ManagementPackClassIds.ToList(),
//                LeastDerivedNonAbstractManagementPackClassId =
//                    enterpriseManagementObject.LeastDerivedNonAbstractManagementPackClassId,
//                TimeAdded = enterpriseManagementObject.TimeAdded,
//                LastModifiedBy = enterpriseManagementObject.LastModifiedBy,
//                LastModified = enterpriseManagementObject.LastModified
//            };

//            var valuesDict = new Dictionary<string, object>();
//            foreach (var enterpriseManagementSimpleObject in enterpriseManagementObject.Values)
//            {
//                var value = enterpriseManagementSimpleObject.Value;
//                if (value is ManagementPackEnumeration en)
//                {
//                    value = en.DisplayName;
//                }

//                if (!valuesDict.ContainsKey(enterpriseManagementSimpleObject.Type.Name))
//                {
//                    valuesDict.Add(enterpriseManagementSimpleObject.Type.Name, value);
//                }
//            }

//            dto.Values = valuesDict;

//            return dto;

//        }



//        public static EnterpriseManagementObjectProjectionDto ToObjectProjectionDto(IComposableProjection composableProjection, int? levels = null)
//        {
            
//            var dto = new EnterpriseManagementObjectProjectionDto
//            {
//                Id = composableProjection.Object.Id,
//                Name = composableProjection.Object.Name,
//                Path = composableProjection.Object.Path,
//                DisplayName = composableProjection.Object.DisplayName,
//                FullName = composableProjection.Object.FullName,
//                ManagementPackClassIds = composableProjection.Object.ManagementPackClassIds.ToList(),
//                LeastDerivedNonAbstractManagementPackClassId =
//                    composableProjection.Object.LeastDerivedNonAbstractManagementPackClassId,
//                TimeAdded = composableProjection.Object.TimeAdded,
//                LastModifiedBy = composableProjection.Object.LastModifiedBy,
//                LastModified = composableProjection.Object.LastModified
//            };

//            var valuesDict = new Dictionary<string, object>();
//            foreach (var enterpriseManagementSimpleObject in composableProjection.Object.Values)
//            {
//                var value = enterpriseManagementSimpleObject.Value;
//                if (value is ManagementPackEnumeration en)
//                {
//                    value = en.DisplayName;
//                }

//                if (!valuesDict.ContainsKey(enterpriseManagementSimpleObject.Type.Name))
//                {
//                    valuesDict.Add(enterpriseManagementSimpleObject.Type.Name, value);
//                }
//            }

//            dto.Values = valuesDict;

//            if (levels.HasValue)
//            {
//                if (levels.Value <= 0)
//                {
//                    return dto;
//                }

//                levels--;
//            }

//            Dictionary<string, List<EnterpriseManagementObjectProjectionDto>> related = new Dictionary<string, List<EnterpriseManagementObjectProjectionDto>>();

//            foreach (var keyValuePair in composableProjection)
//            {
//                var name = keyValuePair.Value.Object.GetClasses(BaseClassTraversalDepth.None).First().Name;
//                if (!related.ContainsKey(name))
//                {
//                    related.Add(name, new List<EnterpriseManagementObjectProjectionDto>());
//                }
                
//                var relatedValue = ToObjectProjectionDto(keyValuePair.Value, levels);
//                related[name].Add(relatedValue);
//            }

//            dto.RelatedObjects = related;

//            return dto;

//        }
//    }
//}
