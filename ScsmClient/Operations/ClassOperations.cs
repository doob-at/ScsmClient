using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;

namespace ScsmClient.Operations
{
    public class ClassOperations: BaseOperation
    {
        public ClassOperations(SCSMClient client) : base(client)
        {
        }

        public ManagementPackClass GetClass(string className, ManagementPack managementPack)
        {
            return _client.ManagementGroup.EntityTypes.GetClass(className, managementPack);
        }

        public ManagementPackClass GetClass(ManagementPackClassCriteria criteria)
        {
            return _client.ManagementGroup.EntityTypes.GetClasses(criteria).FirstOrDefault();
        }

    }
}
