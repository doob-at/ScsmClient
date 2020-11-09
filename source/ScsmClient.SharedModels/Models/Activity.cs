using System;
using System.Collections.Generic;
using System.Text;
using Reflectensions.HelperClasses;

namespace ScsmClient.SharedModels.Models
{
    public class Activity: WorkItem
    {
        public int SequenceId { get; set; }
        public string ChildId { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Area { get; set; }
        public string Stage { get; set; }
        public string Documentation { get; set; }
        public bool Skip { get; set; }

        public Activity() : base()
        {

        }

        public Activity(IDictionary<string, object> dictionary) : base(dictionary)
        {

        }

        public Activity(ExpandableObject expandableObject) : base(expandableObject)
        {

        }
    }
}
