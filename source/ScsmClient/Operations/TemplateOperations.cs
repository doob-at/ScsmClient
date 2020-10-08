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


        public ManagementPackObjectTemplate GetObjectTemplateByName(string templateName)
        {

            var crit = new ManagementPackObjectTemplateCriteria($"DisplayName='{templateName}'");
            return _client.ManagementGroup.Templates.GetObjectTemplates(crit).FirstOrDefault();
        }

    }
}
