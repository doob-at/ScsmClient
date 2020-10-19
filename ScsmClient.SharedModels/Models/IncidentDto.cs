using System.Collections.Generic;
using Reflectensions.HelperClasses;

namespace ScsmClient.SharedModels.Models
{
    public class IncidentDto: ExpandableObject
    {
        public string Title { get; set; }
        public string DisplayName { get; set; }
        public string Urgency { get; set; }
        public string Impact { get; set; }

        public IncidentDto(): base()
        {
            
        }

        public IncidentDto(IDictionary<string, object> dictionary): base(dictionary)
        {
           
        }

        public IncidentDto(ExpandableObject expandableObject) : base(expandableObject)
        {

        }
    }
}
