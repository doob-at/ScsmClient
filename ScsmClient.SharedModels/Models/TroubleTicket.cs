using System;
using System.Collections.Generic;
using Reflectensions.HelperClasses;

namespace ScsmClient.SharedModels.Models
{
    public class TroubleTicket : WorkItem
    {
        public int Priority { get; set; }
        public string Urgency { get; set; }
        public string Impact { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }

        protected TroubleTicket() : base()
        {

        }

        protected TroubleTicket(IDictionary<string, object> dictionary) : base(dictionary)
        {

        }

        protected TroubleTicket(ExpandableObject expandableObject) : base(expandableObject)
        {

        }
    }
}