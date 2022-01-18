using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using doob.Reflectensions.Common;
using doob.Reflectensions.ExtensionMethods;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.CriteriaParser;
using ScsmClient.CriteriaParser.Syntax;
using ScsmClient.Helper;

namespace ScsmClient.Operations
{
    public class CriteriaOperations : BaseOperation
    {
        public CriteriaOperations(SCSMClient client) : base(client)
        {
        }

        public EnterpriseManagementObjectCriteria BuildObjectCriteria(string criteria, ManagementPackClass managementPackClass)
        {
            criteria = criteria ?? "";
            if (!SimpleXml.TryParse(criteria, out var xmlCriteria))
            {
                xmlCriteria = CreateCriteriaXmlFromFilterString(criteria, managementPackClass);
            }

            return xmlCriteria == null ?
                new EnterpriseManagementObjectCriteria("", managementPackClass) :
                new EnterpriseManagementObjectCriteria(xmlCriteria.ToString(), managementPackClass, _client.ManagementGroup);
        }

        public ObjectProjectionCriteria BuildObjectProjectionCriteria(string criteria, ManagementPackTypeProjection typeProjection)
        {
            criteria = criteria ?? "";
            if (!SimpleXml.TryParse(criteria, out var xmlCriteria))
            {
                xmlCriteria = CreateCriteriaXmlFromFilterString(criteria, typeProjection);
            }

            if (xmlCriteria == null)
            {
                return new ObjectProjectionCriteria(typeProjection);
            }



            return new ObjectProjectionCriteria(xmlCriteria.ToString(), typeProjection, _client.ManagementGroup);
        }


        public SimpleXml CreateCriteriaXmlFromFilterString(string filter, ManagementPackClass managementPackClass)
        {
            if (String.IsNullOrWhiteSpace(filter))
            {
                return null;
            }
            var syntaxTree = SyntaxTree.Parse(filter);
            var compilation = new Compilation(syntaxTree, _client);
            var result = compilation.Evaluate(managementPackClass);
            if (result.Diagnostics.Any())
            {
                throw new Exception(String.Join(Environment.NewLine, result.Diagnostics.Select(d => d.Message)));
            }
            return result.Value;
        }

        public SimpleXml CreateCriteriaXmlFromFilterString(string filter, ManagementPackTypeProjection typeProjection)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return null;
            }
            var syntaxTree = SyntaxTree.Parse(filter);
            var compilation = new Compilation(syntaxTree, _client);
            var result = compilation.Evaluate(typeProjection);
            if (result.Diagnostics.Any())
            {
                throw new Exception(String.Join(Environment.NewLine, result.Diagnostics.Select(d => d.Message)));
            }
            return result.Value;
        }


        public SimpleXml GetCriteriaFromTypeNameAsXml(string typeName, string criteria)
        {
            var objectClass = _client.Types().GetClassByName(typeName);
            if (objectClass != null)
            {
                var xmlstr = _client.Criteria().BuildObjectCriteria(criteria, objectClass).CriteriaXml;
                return SimpleXml.Parse(xmlstr);
            }
            var typeProjectionClass = _client.Types().GetTypeProjectionByName(typeName);
            if (typeProjectionClass != null)
            {
                var c = _client.Criteria().BuildObjectProjectionCriteria(criteria, typeProjectionClass);
                var xmlstr = c.Reflect().GetPropertyValue<string>("Criteria", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                return SimpleXml.Parse(xmlstr);
            }

            throw new Exception($"Type '{typeName}' not found");
        }

        public string CreateSortCriteriaXmlFrom(string sortProperty, ManagementPackClass managementPackClass)
        {
            var order = SortingOrder.Ascending;
            if (sortProperty.StartsWith("-"))
            {
                order = SortingOrder.Descending;
                sortProperty = sortProperty.Substring(1);
            }

            return CreateSortCriteriaXmlFrom(sortProperty, order, managementPackClass);
        }

        public string CreateSortCriteriaXmlFrom(string sortProperty, SortingOrder order, ManagementPackClass managementPackClass)
        {

            string prefix = null;
            if (sortProperty.StartsWith("G:", StringComparison.OrdinalIgnoreCase))
            {
                prefix = "G";
                sortProperty = sortProperty.Substring(2);
            }
            else if (sortProperty.StartsWith("P:", StringComparison.OrdinalIgnoreCase))
            {
                prefix = "P";
                sortProperty = sortProperty.Substring(2);
            }


            if (prefix == null || prefix == "P")
            {
                foreach (var managementPackProperty in managementPackClass.GetProperties(BaseClassTraversalDepth.Recursive))
                {

                    if (managementPackProperty.Name.Equals(sortProperty, StringComparison.OrdinalIgnoreCase))
                    {
                        return $"<Sorting xmlns=\"http://Microsoft.EnterpriseManagement.Core.Sorting\"><SortProperty SortOrder=\"{order.GetName()}\">$Target/Property[Type='{managementPackClass.Id}']/{managementPackProperty.Name}$</SortProperty></Sorting>";
                    }
                }
            }

            if (prefix == null || prefix == "G")
            {
                foreach (var genericProperty in GenericProperty.GetGenericProperties())
                {
                    if (genericProperty.PropertyName.Equals(sortProperty, StringComparison.OrdinalIgnoreCase))
                    {
                        return
                            $"<Sorting xmlns=\"http://Microsoft.EnterpriseManagement.Core.Sorting\"><GenericSortProperty SortOrder=\"{order.GetName()}\">{genericProperty.PropertyName}</GenericSortProperty></Sorting>";
                    }
                }
            }

            return null;
        }


        public string CreateSortCriteriaXmlFrom(string sortProperty, ManagementPackTypeProjection managementPackTypeProjection)
        {
            var order = SortingOrder.Ascending;
            if (sortProperty.StartsWith("-"))
            {
                order = SortingOrder.Descending;
                sortProperty = sortProperty.Substring(1);
            }

            return CreateSortCriteriaXmlFrom(sortProperty, order, managementPackTypeProjection.TargetType);
        }
        public string CreateSortCriteriaXmlFrom(string sortProperty, SortingOrder order, ManagementPackTypeProjection managementPackTypeProjection)
        {
            return CreateSortCriteriaXmlFrom(sortProperty, order, managementPackTypeProjection.TargetType);
            //foreach (var genericProperty in GenericProperty.GetGenericProperties())
            //{
            //    if (genericProperty.PropertyName.Equals(sortProperty, StringComparison.OrdinalIgnoreCase))
            //    {
            //        return $"<Sorting xmlns=\"http://Microsoft.EnterpriseManagement.Core.Sorting\"><GenericSortProperty SortOrder=\"{order}\">{genericProperty.PropertyName}</GenericSortProperty></Sorting>";
            //    }
            //}

            //foreach (var managementPackProperty in managementPackTypeProjection.TargetType.GetProperties(BaseClassTraversalDepth.Recursive))
            //{

            //    if (managementPackProperty.Name.Equals(sortProperty, StringComparison.OrdinalIgnoreCase))
            //    {
            //        return $"<Sorting xmlns=\"http://Microsoft.EnterpriseManagement.Core.Sorting\"><SortProperty SortOrder=\"{order}\">$Target/Property[Type='{managementPackTypeProjection.TargetType.Id}']/{managementPackProperty.Name}$</SortProperty></Sorting>";
            //    }
            //}

            //return null;
        }
    }
}
