using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.CriteriaParser;
using ScsmClient.CriteriaParser.Syntax;
using ScsmClient.Helper;

namespace ScsmClient.Operations
{
    public class CriteriaOperations: BaseOperation
    {
        public CriteriaOperations(SCSMClient client) : base(client)
        {
        }

        public EnterpriseManagementObjectCriteria BuildObjectCriteria(string criteria, ManagementPackClass managementPackClass)
        {
            
            if(!SimpleXml.TryParse(criteria, out var xmlCriteria))
            {
                xmlCriteria = CreateCriteriaXmlFromFilterString(criteria, managementPackClass);
            }

            return xmlCriteria == null ?
                new EnterpriseManagementObjectCriteria("", managementPackClass) :
                new EnterpriseManagementObjectCriteria(xmlCriteria.ToString(), managementPackClass, _client.ManagementGroup);
        }

        public ObjectProjectionCriteria BuildObjectProjectionCriteria(string criteria, ManagementPackTypeProjection typeProjection)
        {
            if (!SimpleXml.TryParse(criteria, out var xmlCriteria))
            {
                xmlCriteria = CreateCriteriaXmlFromFilterString(criteria, typeProjection);
            }

            return xmlCriteria == null ?
                new ObjectProjectionCriteria(typeProjection) :
                new ObjectProjectionCriteria(xmlCriteria.ToString(), typeProjection, _client.ManagementGroup);
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

    }
}
