using System;
using System.Collections.Generic;
using System.Linq;
using Expandable.ExtensionMethods;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Operations
{
    public class ChangeRequestOperations : BaseOperation
    {
        public ChangeRequestOperations(SCSMClient client) : base(client)
        {
        }

        public ChangeRequestDto GetByGenericId(Guid id)
        {
            var entObj = _client.Object().GetObjectById(id);
            return new ChangeRequestDto(entObj.Values);
        }

        public ChangeRequestDto GetById(string id)
        {
            return GetByCriteria($"Id == '{id}'", 1).FirstOrDefault();
        }

        public List<ChangeRequestDto> GetByCriteria(string criteria, int? maxResults = null)
        {
            var srObjs = _client.TypeProjection().GetObjectProjectionObjects(WellKnown.ChangeRequest.ProjectionType, criteria, maxResults).ToList();
            return srObjs.Select(e => new ChangeRequestDto(e.Values)).ToList();
        }

        public ChangeRequestDto Create(ChangeRequestDto changeReuest)
        {
            var entObj = _client.Object().CreateObjectByClassId(WellKnown.ChangeRequest.ClassId, changeReuest.AsDictionary());
            return new ChangeRequestDto(entObj.Values);
        }

        public ChangeRequestDto CreateFromTemplate(string templateName, ChangeRequestDto changeReuest)
        {
            var template = _client.Template().GetObjectTemplateByName(templateName);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                if (managementPackTypeProjection.TargetType.Id != WellKnown.ChangeRequest.ClassId)
                {
                    throw new Exception($"Template '{templateName}' is not a ChangeRequest Template!");
                }

                var entObj = _client.Object().CreateObjectFromTemplate(template, changeReuest.AsDictionary());
                return new ChangeRequestDto(entObj.Values);
            }
            else
            {
                throw new Exception($"Template '{templateName}' is not a ChangeRequest Template!");
            }

        }


    }

    
}
