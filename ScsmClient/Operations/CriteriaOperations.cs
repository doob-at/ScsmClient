using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;

namespace BaseIT.SCSM.Client.Operations
{
    public class CriteriaOperations: BaseOperation
    {
        public CriteriaOperations(SCSMClient client) : base(client)
        {
        }

        public ObjectProjectionCriteria BuildObjectProjectionCriteria(string criteria, ManagementPackTypeProjection typeProjection)
        {
            return new ObjectProjectionCriteria(criteria, typeProjection, _client.ManagementGroup);
        }

        public ManagementPackTypeProjectionCriteria BuildManagementPackTypeProjectionCriteria(string criteria)
        {
            
            return new ManagementPackTypeProjectionCriteria(criteria);
        }
    }
}
