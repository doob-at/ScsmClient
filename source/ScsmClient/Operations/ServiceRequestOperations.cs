using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.ExtensionMethods;
using ScsmClient.Model;
using ScsmClient.SharedModels;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Operations
{
    public class ServiceRequestOperations : BaseOperation
    {
        public ServiceRequestOperations(SCSMClient client) : base(client)
        {
        }

        public ServiceRequest GetByGenericId(Guid id, int? levels = null)
        {
            if (levels.HasValue && levels.Value == 0)
            {
                return _client.ScsmObject().GetObjectById(id).To<ServiceRequest>();
            }
            var retOptions = new RetrievalOptions();
            retOptions.ReferenceLevels = levels;
            retOptions.MaxResultCount = 1;
            return GetByCriteria($"@G:System.WorkItem.ServiceRequest!Id == '{id}'", retOptions).FirstOrDefault();
        }

        public ServiceRequest GetById(string id, int? levels = null)
        {
            var retOptions = new RetrievalOptions();
            retOptions.ReferenceLevels = levels;
            retOptions.MaxResultCount = 1;
            return GetByCriteria($"@Id == '{id}'", retOptions).FirstOrDefault();
        }

        public List<ServiceRequest> GetByCriteria(string criteria, RetrievalOptions retrievalOptions = null)
        {
            var srObjs = _client.ScsmObject().GetObjectsByTypeId(WellKnown.ServiceRequest.ProjectionType, criteria, retrievalOptions);
            return srObjs.Select(o => o.To<ServiceRequest>()).ToList();
        }

        public Guid Create(ServiceRequest serviceRequest)
        {
            var id = _client.Object().CreateObjectByClassId(WellKnown.ServiceRequest.ClassId, serviceRequest.AsDictionary());
            return id;
        }

        public Guid CreateFromTemplate(string templateName, ServiceRequest serviceRequest)
        {
            var template = _client.Template().GetObjectTemplateByName(templateName);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                if (managementPackTypeProjection.TargetType.Id != WellKnown.ServiceRequest.ClassId)
                {
                    throw new Exception($"Template '{templateName}' is not a ServiceRequest Template!");
                }

                var id = _client.Object().CreateObjectFromTemplate(template, serviceRequest.AsDictionary());
                return id;
            }
            else
            {
                throw new Exception($"Template '{templateName}' is not a ServiceRequest Template!");
            }

        }


    }

    
}
