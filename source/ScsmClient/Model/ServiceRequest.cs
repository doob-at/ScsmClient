using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScsmClient.Model
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

    }
}
