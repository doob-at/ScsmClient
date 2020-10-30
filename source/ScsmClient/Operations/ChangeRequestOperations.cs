using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EnterpriseManagement.Configuration;
using Reflectensions.ExtensionMethods;
using ScsmClient.SharedModels;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Operations
{
    public class ChangeRequestOperations : BaseOperation
    {
        public ChangeRequestOperations(SCSMClient client) : base(client)
        {
        }

        public ChangeRequest GetByGenericId(Guid id, int? levels = null)
        {
            if (levels.HasValue && levels.Value == 0)
            {
                return _client.ScsmObject().GetObjectById(id).SwitchType<ChangeRequest>();
            }
            return GetByCriteria($"G:System.WorkItem.ChangeRequest!Id == '{id}'", 1, levels).FirstOrDefault();
        }

        public ChangeRequest GetById(string id, int? levels = null)
        {
            return GetByCriteria($"Id == '{id}'", 1, levels).FirstOrDefault();
        }

        public List<ChangeRequest> GetByCriteria(string criteria, int? maxResults = null, int? levels = null)
        {
            var srObjs = _client.ScsmObject().GetObjectsByTypeId(WellKnown.ChangeRequest.ProjectionType, criteria, maxResults, levels);
            return srObjs.SwitchType<ChangeRequest>().ToList();
        }

        public Guid Create(ChangeRequest changeReuest)
        {
            var id = _client.Object().CreateObjectByClassId(WellKnown.ChangeRequest.ClassId, changeReuest.AsDictionary());
            return id;
        }

        public Guid CreateFromTemplate(string templateName, ChangeRequest changeReuest)
        {
            var template = _client.Template().GetObjectTemplateByName(templateName);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                if (managementPackTypeProjection.TargetType.Id != WellKnown.ChangeRequest.ClassId)
                {
                    throw new Exception($"Template '{templateName}' is not a ChangeRequest Template!");
                }

                var id = _client.Object().CreateObjectFromTemplate(template, changeReuest.AsDictionary());
                return id;
            }
            else
            {
                throw new Exception($"Template '{templateName}' is not a ChangeRequest Template!");
            }

        }

        public void UpdateByGenericId(Guid changeId, Dictionary<string, object> properties)
        {
            _client.Object().UpdateObject(changeId, properties);
        }

        public void UpdateById(string id, Dictionary<string, object> properties)
        {
            var ch = GetById(id);
            _client.Object().UpdateObject(ch.ObjectId, properties);
        }
    }

    
}
