using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Expandable.ExtensionMethods;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.ExtensionMethods;
using ScsmClient.Model;

namespace ScsmClient.Operations
{
    public class IncidentOperations : BaseOperation
    {
        public IncidentOperations(SCSMClient client) : base(client)
        {
        }

        public EnterpriseManagementObjectDto Create(Incident incident)
        {
            return _client.Object().CreateObject(WellKnown.Incident.ClassId, incident.AsDictionary());
        }
        
        public EnterpriseManagementObjectDto CreateFromTemplate(string templateName, Incident incident)
        {
            var template = _client.Template().GetObjectTemplateByName(templateName);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                if (managementPackTypeProjection.TargetType.Id != WellKnown.Incident.ClassId)
                {
                    throw new Exception($"Template '{templateName}' is not an Incident Template!");
                }

                return _client.Object().CreateObjectFromTemplate(template, incident.AsDictionary());
            }
            else
            {
                throw new Exception($"Template '{templateName}' is not an Incident Template!");
            }

        }

    }
    
}
