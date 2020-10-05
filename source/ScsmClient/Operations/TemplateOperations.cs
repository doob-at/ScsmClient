using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;

namespace ScsmClient.Operations
{
    public class TemplateOperations: BaseOperation
    {
        public TemplateOperations(SCSMClient client) : base(client)
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

        public ManagementPackObjectTemplate GetObjectTemplateByName(string templateName)
        {

            var crit = new ManagementPackObjectTemplateCriteria($"Name='{templateName}'");
            return _client.ManagementGroup.Templates.GetObjectTemplates(crit).FirstOrDefault();
        }

    }
}
