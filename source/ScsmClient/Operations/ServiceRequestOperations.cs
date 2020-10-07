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
    public class ServiceRequestOperations : BaseOperation
    {
        public ServiceRequestOperations(SCSMClient client) : base(client)
        {
        }

        public EnterpriseManagementObjectDto Create(ServiceRequestDto serviceRequest)
        {
            return _client.Object().CreateObject(WellKnown.ServiceRequest.ClassId, serviceRequest.AsDictionary());
        }

        
        public EnterpriseManagementObjectDto CreateFromTemplate(string templateName, ServiceRequestDto serviceRequest)
        {

         
            var template = _client.Template().GetObjectTemplateByName(templateName);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                if (managementPackTypeProjection.TargetType.Id != WellKnown.Incident.ClassId)
                {
                    throw new Exception($"Template '{templateName}' is not an ServiceRequest Template!");
                }

                return _client.Object().CreateObjectFromTemplate(template, serviceRequest.AsDictionary());
            }
            else
            {
                throw new Exception($"Template '{templateName}' is not an ServiceRequest Template!");
            }
            

            

        }


    }

    
}
