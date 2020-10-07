using System;
using System.Collections.Generic;

namespace ScsmClient.SharedModels.Models
{
    public class ServiceRequestDto: WorkItemDto
    {
        public string Status { get; set; }
        public string TemplateId { get; set; }
        public string Priority { get; set; }
        public string Urgency { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string Source { get; set; }
        public string ImplementationResults { get; set; }
        public string Notes { get; set; }
        public string Area { get; set; }
        public string SupportGroup { get; set; }

        public ServiceRequestDto() : base()
        {

        }

        public ServiceRequestDto(Dictionary<string, object> dictionary) : base(dictionary)
        {

        }
    }
}
