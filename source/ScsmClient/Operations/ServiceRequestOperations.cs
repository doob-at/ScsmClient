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
    public class ServiceRequestOperations : BaseOperation
    {
        public ServiceRequestOperations(SCSMClient client) : base(client)
        {
        }

        public ServiceRequestDto GetByGenericId(Guid id)
        {
            var entObj = _client.Object().GetObjectById(id);
            return new ServiceRequestDto(entObj);
        }

        public ServiceRequestDto GetById(string id)
        {
            return GetByCriteria($"Id == '{id}'", 1).FirstOrDefault();
        }

        public List<ServiceRequestDto> GetByCriteria(string criteria, int? maxResults = null)
        {
            var srObjs = _client.TypeProjection().GetObjectProjectionObjects(WellKnown.ServiceRequest.ProjectionType, criteria, maxResults).ToList();
            return srObjs.Select(e => new ServiceRequestDto(e)).ToList();
        }

        public Guid Create(ServiceRequestDto serviceReuest)
        {
            var id = _client.Object().CreateObjectByClassId(WellKnown.ServiceRequest.ClassId, serviceReuest.AsDictionary());
            return id;
        }

        public Guid CreateFromTemplate(string templateName, ServiceRequestDto serviceReuest)
        {
            var template = _client.Template().GetObjectTemplateByName(templateName);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                if (managementPackTypeProjection.TargetType.Id != WellKnown.ServiceRequest.ClassId)
                {
                    throw new Exception($"Template '{templateName}' is not a ServiceRequest Template!");
                }

                var id = _client.Object().CreateObjectFromTemplate(template, serviceReuest.AsDictionary());
                return id;
            }
            else
            {
                throw new Exception($"Template '{templateName}' is not a ServiceRequest Template!");
            }

        }


    }

    
}
