using System;
using System.Collections.Generic;
using System.Linq;
using doob.Reflectensions.Common;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.SharedModels;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.ExtensionMethods
{
    public static class EnterpriseManagementObjectExtensions
    {

        //public static EnterpriseManagementObjectDto ToObjectProjectionDto(this IComposableProjection composableProjection, int? levels = null)
        //{
        //    var dto = new EnterpriseManagementObjectDto
        //    {
        //        Id = composableProjection.Object.Id,
        //        Name = composableProjection.Object.Name,
        //        Path = composableProjection.Object.Path,
        //        DisplayName = composableProjection.Object.DisplayName,
        //        FullName = composableProjection.Object.FullName,
        //        ManagementPackClassIds = composableProjection.Object.ManagementPackClassIds.ToList(),
        //        LeastDerivedNonAbstractManagementPackClassId =
        //           composableProjection.Object.LeastDerivedNonAbstractManagementPackClassId,
        //        TimeAdded = composableProjection.Object.TimeAdded,
        //        LastModifiedBy = composableProjection.Object.LastModifiedBy,
        //        LastModified = composableProjection.Object.LastModified
        //    };

        //    var valuesDict = new Dictionary<string, object>();
        //    foreach (var enterpriseManagementSimpleObject in composableProjection.Object.Values)
        //    {
        //        var value = enterpriseManagementSimpleObject.Value;
        //        if (value is ManagementPackEnumeration en)
        //        {
        //            value = en.DisplayName;
        //        }

        //        if (!valuesDict.ContainsKey(enterpriseManagementSimpleObject.Type.Name))
        //        {
        //            valuesDict.Add(enterpriseManagementSimpleObject.Type.Name, value);
        //        }
        //    }

        //    dto.Values = valuesDict;

        //    if (levels.HasValue)
        //    {
        //        if (levels.Value <= 0)
        //        {
        //            return dto;
        //        }

        //        levels--;
        //    }

        //    Dictionary<string, List<EnterpriseManagementObjectDto>> related = new Dictionary<string, List<EnterpriseManagementObjectDto>>();

        //    foreach (var keyValuePair in composableProjection)
        //    {
        //        var name = keyValuePair.Value.Object.GetClasses(BaseClassTraversalDepth.None).First().Name;
        //        if (!related.ContainsKey(name))
        //        {
        //            related.Add(name, new List<EnterpriseManagementObjectDto>());
        //        }

        //        var relatedValue = ToObjectProjectionDto(keyValuePair.Value, levels);
        //        related[name].Add(relatedValue);
        //    }
            
        //    foreach (var pair in related)
        //    {
        //        dto.Values[$"!{pair.Key}"] = pair.Value;
        //    }

        //    return dto;
        //}

        //public static EnterpriseManagementObjectDto ToObjectDto(this EnterpriseManagementObject enterpriseManagementObject)
        //{
        //    var dto = new EnterpriseManagementObjectDto
        //    {
        //        Id = enterpriseManagementObject.Id,
        //        Name = enterpriseManagementObject.Name,
        //        Path = enterpriseManagementObject.Path,
        //        DisplayName = enterpriseManagementObject.DisplayName,
        //        FullName = enterpriseManagementObject.FullName,
        //        ManagementPackClassIds = enterpriseManagementObject.ManagementPackClassIds.ToList(),
        //        LeastDerivedNonAbstractManagementPackClassId =
        //            enterpriseManagementObject.LeastDerivedNonAbstractManagementPackClassId,
        //        TimeAdded = enterpriseManagementObject.TimeAdded,
        //        LastModifiedBy = enterpriseManagementObject.LastModifiedBy,
        //        LastModified = enterpriseManagementObject.LastModified
        //    };

        //    var valuesDict = new Dictionary<string, object>();
        //    foreach (var enterpriseManagementSimpleObject in enterpriseManagementObject.Values)
        //    {
        //        var value = enterpriseManagementSimpleObject.Value;
        //        if (value is ManagementPackEnumeration en)
        //        {
        //            value = en.DisplayName;
        //        }

        //        if (!valuesDict.ContainsKey(enterpriseManagementSimpleObject.Type.Name))
        //        {
        //            valuesDict.Add(enterpriseManagementSimpleObject.Type.Name, value);
        //        }
        //    }

        //    dto.Values = valuesDict;

        //    return dto;
        //}

        
        public static ScsmObject ToScsmObject(this IComposableProjection composableProjection,
            ManagementPackTypeProjection managementPackTypeProjection, int? levels = null)
        {
            var dto = new ScsmObject()
            {
                ObjectId = composableProjection.Object.Id,
                TimeAdded = composableProjection.Object.TimeAdded,
                LastModified = composableProjection.Object.LastModified
            };

            dto.SetEnterpriseManagementSimpleObjectValues(composableProjection.Object.Values);
            dto["@class"] = composableProjection.Object.GetManagementPackClassName();

            if (levels.HasValue)
            {
                if (levels.Value <= 0)
                {
                    return dto;
                }

                levels--;
            }

            Dictionary<string, List<ScsmObject>> related = new Dictionary<string, List<ScsmObject>>();


            
            foreach (var keyValuePair in composableProjection)
            {

                
                var k = keyValuePair.Key;
                //var hasName = !String.IsNullOrWhiteSpace(k.ParentElement?.Name);

                var name = FindAlias(managementPackTypeProjection, k.Id);
                //var name = hasName ? k.ParentElement.Name : $"{keyValuePair.Value.Object.GetManagementPackClassName()}!";

                var isArray = k.MaxCardinality > 1;

                if (!isArray)
                {
                    dto[$"{name}"] = ToScsmObject(keyValuePair.Value, managementPackTypeProjection, levels);
                }
                else
                {
                    if (!related.ContainsKey(name))
                    {
                        related.Add(name, new List<ScsmObject>());
                    }

                    var relatedValue = ToScsmObject(keyValuePair.Value,managementPackTypeProjection,  levels);
                    related[name].Add(relatedValue);
                }

            }

            foreach (var pair in related)
            {
                dto[$"{pair.Key}"] = pair.Value;
            }

            return dto;
        }

        private static string FindAlias(ITypeProjectionComponent managementPackTypeProjection, Guid n)
        {

            foreach (var keyValuePair in managementPackTypeProjection)
            {
                var g = n;

                if (keyValuePair.Key.Id == n)
                {
                    return keyValuePair.Value.Alias;
                }

                if (keyValuePair.Value.Count() > 0)
                {
                    foreach (var valuePair in keyValuePair.Value)
                    {
                        var alias = FindAlias(valuePair, n);
                        if (!String.IsNullOrWhiteSpace(alias))
                        {
                            return alias;
                        }
                    }
                }
            }

            return null;
        }

        private static string FindAlias(KeyValuePair<ManagementPackRelationshipEndpoint, ITypeProjectionComponent> keyValuePair, Guid guid)
        {
            if (keyValuePair.Key.Id == guid)
            {
                return keyValuePair.Value.Alias.ToNull() ?? keyValuePair.Key.ParentElement.Name;
            }

            if (keyValuePair.Value.Count() > 0)
            {
                foreach (var valuePair in keyValuePair.Value)
                {
                    var alias = FindAlias(valuePair, guid);
                    if (!String.IsNullOrWhiteSpace(alias))
                    {
                        return alias;
                    }
                }
            }

            return null;
        }

        public static ScsmObject ToScsmObject(this EnterpriseManagementObject enterpriseManagementObject)
        {
            var dto = new ScsmObject()
            {
                ObjectId = enterpriseManagementObject.Id,
                TimeAdded = enterpriseManagementObject.TimeAdded,
                LastModified = enterpriseManagementObject.LastModified
            };
            
            dto.SetEnterpriseManagementSimpleObjectValues(enterpriseManagementObject.Values);
            dto["@class"] = enterpriseManagementObject.GetManagementPackClassName();
            return dto;
        }

        private static ScsmObject SetEnterpriseManagementSimpleObjectValues(this ScsmObject scsmObject, IList<EnterpriseManagementSimpleObject> simpleObjects)
        {
            foreach (var enterpriseManagementSimpleObject in simpleObjects)
            {
                var value = enterpriseManagementSimpleObject.Value;
                if (value is ManagementPackEnumeration en)
                {
                    value = en.DisplayName;
                }

                if (!scsmObject.ContainsKey(enterpriseManagementSimpleObject.Type.Name))
                {
                    if (enterpriseManagementSimpleObject.Type.ParentElement.Id == WellKnown.WorkItem.ClassId &&
                        enterpriseManagementSimpleObject.Type.Name.Equals("UserInput", StringComparison.OrdinalIgnoreCase))
                    {

                        scsmObject[enterpriseManagementSimpleObject.Type.Name] = UserInput.FromXml(value?.ToString());
                        continue;
                    }
                    scsmObject[enterpriseManagementSimpleObject.Type.Name] = value;
                }
            }

            return scsmObject;
        }


        public static ManagementPackClass GetManagementPackClass(this EnterpriseManagementObject enterpriseManagementObject)
        {
            return enterpriseManagementObject.GetMostDerivedClasses().First();
        }

        public static string GetManagementPackClassName(this EnterpriseManagementObject enterpriseManagementObject)
        {
            return enterpriseManagementObject.GetManagementPackClass().Name;
        }
    }
}
