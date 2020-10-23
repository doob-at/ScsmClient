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

        public ChangeRequestDto GetByGenericId(Guid id)
        {
            var entObj = _client.ScsmObject().GetObjectById(id);
            return new ChangeRequestDto(entObj);
        }

        public ChangeRequestDto GetById(string id)
        {
            return GetByCriteria($"Id == '{id}'", 1).FirstOrDefault();
        }

        public List<ChangeRequestDto> GetByCriteria(string criteria, int? maxResults = null)
        {
            var srObjs = _client.ScsmObject().GetObjectsByTypeId(WellKnown.ChangeRequest.ProjectionType, criteria, maxResults).ToList();
            return srObjs.Select(e => new ChangeRequestDto(e)).ToList();
        }

        public Guid Create(ChangeRequestDto changeReuest)
        {
            var id = _client.Object().CreateObjectByClassId(WellKnown.ChangeRequest.ClassId, changeReuest.AsDictionary());
            return id;
        }

        public Guid CreateFromTemplate(string templateName, ChangeRequestDto changeReuest)
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


    }

    
}
