using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Expandable.ExtensionMethods;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.ExtensionMethods;
using ScsmClient.SharedModels;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Operations
{
    public class IncidentOperations : BaseOperation
    {
        public IncidentOperations(SCSMClient client) : base(client)
        {
        }

        public IncidentDto GetById(Guid id)
        {
            var entObj = _client.Object().GetObjectById(id);
            return new IncidentDto(entObj.Values);
        }

        public List<IncidentDto> GetByCriteria(string criteria, int? maxResults = null)
        {
            var incObjs = _client.TypeProjection().GetObjectProjectionObjects(WellKnown.Incident.ProjectionType, criteria, maxResults).ToList();
            return incObjs.Select(e => new IncidentDto(e.Values)).ToList();
        }

        public IncidentDto Create(IncidentDto incident)
        {
            var entObj = _client.Object().CreateObjectByClassId(WellKnown.Incident.ClassId, incident.AsDictionary());
            return new IncidentDto(entObj.Values);
        }
        
        public IncidentDto CreateFromTemplate(string templateName, IncidentDto incident)
        {
            var template = _client.Template().GetObjectTemplateByName(templateName);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                if (managementPackTypeProjection.TargetType.Id != WellKnown.Incident.ClassId)
                {
                    throw new Exception($"Template '{templateName}' is not an Incident Template!");
                }

                var entObj = _client.Object().CreateObjectFromTemplate(template, incident.AsDictionary());
                return new IncidentDto(entObj.Values);
            }
            else
            {
                throw new Exception($"Template '{templateName}' is not an Incident Template!");
            }

        }

    }
    
}
