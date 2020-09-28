using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.CriteriaParser;
using ScsmClient.CriteriaParser.Syntax;

namespace ScsmClient.Operations
{
    public class CriteriaOperations: BaseOperation
    {
        public CriteriaOperations(SCSMClient client) : base(client)
        {
        }

        public EnterpriseManagementObjectCriteria BuildObjectCriteria(string criteria, ManagementPackClass managementPackClass)
        {
            return new EnterpriseManagementObjectCriteria(criteria, managementPackClass, _client.ManagementGroup);
           
        }

        public ObjectProjectionCriteria BuildObjectProjectionCriteria(string criteria, ManagementPackTypeProjection typeProjection)
        {
            return new ObjectProjectionCriteria(criteria, typeProjection, _client.ManagementGroup);
        }
        
        public ManagementPackTypeProjectionCriteria BuildManagementPackTypeProjectionCriteria(string criteria)
        {
            return new ManagementPackTypeProjectionCriteria(criteria);
        }

        public ManagementPackClassCriteria BuildManagementPackClassCriteria(string criteria)
        {
            return new ManagementPackClassCriteria(criteria);
        }

        public string CreateCriteriaXmlFromFilterString(string filter, ManagementPackTypeProjection typeProjection)
        {
            var rels = typeProjection.GetManagementPack().GetRelationships();
            var syntaxTree = SyntaxTree.Parse(filter);
            var compilation = new Compilation(syntaxTree);
            var result = compilation.Evaluate();
            return result.Value;
        }
    }
}
