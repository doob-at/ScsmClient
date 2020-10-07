using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expandable;

namespace ScsmClient.Model
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
