using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;
using Reflectensions.ExtensionMethods;
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

       
        public Incident GetByGenericId(Guid id, int? levels = null)
        {
            if (levels.HasValue && levels.Value == 0)
            {
                return _client.ScsmObject().GetObjectById(id).SwitchType<Incident>();
            }
            return GetByCriteria($"@G:System.WorkItem.Incident!Id == '{id}'", 1, levels).FirstOrDefault();
        }

        public Incident GetById(string id, int? levels = null)
        {
            return GetByCriteria($"@Id == '{id}'", 1, levels).FirstOrDefault();
        }

        public List<Incident> GetByCriteria(string criteria, int? maxResults = null, int? levels = null)
        {
            var incObjs = _client.ScsmObject().GetObjectsByTypeId(WellKnown.Incident.ProjectionType, criteria, maxResults, levels).ToList();
            return incObjs.Select(e => new Incident(e)).ToList();
        }

        public Guid Create(Incident incident)
        {
            var id = _client.Object().CreateObjectByClassId(WellKnown.Incident.ClassId, incident.AsDictionary());
            return id;
        }
        
        public Guid CreateFromTemplate(string templateName, Incident incident)
        {
            var template = _client.Template().GetObjectTemplateByName(templateName);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                if (managementPackTypeProjection.TargetType.Id != WellKnown.Incident.ClassId)
                {
                    throw new Exception($"Template '{templateName}' is not an Incident Template!");
                }

                var id = _client.Object().CreateObjectFromTemplate(template, incident.AsDictionary());
                return id;
            }
            else
            {
                throw new Exception($"Template '{templateName}' is not an Incident Template!");
            }

        }

    }
    
}
