using System;
using System.Collections.Generic;
using Reflectensions.HelperClasses;

namespace ScsmClient.SharedModels.Models
{
    public class Incident: TroubleTicket
    {
        public DateTime? TargetResolutionTime { get; set; }
        public bool Escalated { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public string ResolutionDescription { get; set; }
        public bool NeedsKnowledgeArticle { get; set; }
        public string TierQueue { get; set; }
        public bool HasCreatedKnowledgeArticle { get; set; }
        public string LastModifiedSource { get; set; }
        public string Classification { get; set; }
        public string ResolutionCategory { get; set; }

        public Incident(): base()
        {
            
        }

        public Incident(IDictionary<string, object> dictionary): base(dictionary)
        {
           
        }

        public Incident(ExpandableObject expandableObject) : base(expandableObject)
        {

        }
    }
}
