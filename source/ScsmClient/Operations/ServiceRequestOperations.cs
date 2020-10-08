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

        public ServiceRequestDto GetById(Guid id)
        {
            var entObj = _client.Object().GetObjectById(id);
            return new ServiceRequestDto(entObj.Values);
        }

        public List<ServiceRequestDto> GetByCriteria(string criteria, int? maxResults = null)
        {
            var srObjs = _client.TypeProjection().GetObjectProjectionObjects(WellKnown.ServiceRequest.ProjectionType, criteria, maxResults).ToList();
            return srObjs.Select(e => new ServiceRequestDto(e.Values)).ToList();
        }

        public ServiceRequestDto Create(ServiceRequestDto serviceReuest)
        {
            var entObj = _client.Object().CreateObjectByClassId(WellKnown.ServiceRequest.ClassId, serviceReuest.AsDictionary());
            return new ServiceRequestDto(entObj.Values);
        }

        public ServiceRequestDto CreateFromTemplate(string templateName, ServiceRequestDto serviceReuest)
        {
            var template = _client.Template().GetObjectTemplateByName(templateName);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                if (managementPackTypeProjection.TargetType.Id != WellKnown.ServiceRequest.ClassId)
                {
                    throw new Exception($"Template '{templateName}' is not a ServiceRequest Template!");
                }

                var entObj = _client.Object().CreateObjectFromTemplate(template, serviceReuest.AsDictionary());
                return new ServiceRequestDto(entObj.Values);
            }
            else
            {
                throw new Exception($"Template '{templateName}' is not a ServiceRequest Template!");
            }

        }


    }

    
}
