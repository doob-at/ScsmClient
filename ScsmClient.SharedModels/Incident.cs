using System.Collections.Generic;
using Expandable;

namespace ScsmClient.SharedModels
{
    public class Incident: ExpandableObject
    {
        public string Title { get; set; }
        public string DisplayName { get; set; }
        public string Urgency { get; set; }
        public string Impact { get; set; }

        public Incident(): base()
        {
            
        }

        public Incident(Dictionary<string, object> dictionary): base(dictionary)
        {
            
        }
    }
}
