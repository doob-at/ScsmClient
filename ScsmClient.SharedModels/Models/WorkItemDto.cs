using System;
using System.Collections.Generic;
using Reflectensions.HelperClasses;

namespace ScsmClient.SharedModels.Models
{
    public class WorkItemDto: ExpandableObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContactMethod { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public DateTime? ScheduledEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public bool IsDowntime { get; set; }
        public bool IsParent { get; set; }
        public DateTime? ScheduledDowntimeStartDate { get; set; }
        public DateTime? ScheduledDowntimeEndDate { get; set; }
        public DateTime? ActualDowntimeStartDate { get; set; }
        public DateTime? ActualDowntimeEndDate { get; set; }
        public DateTime? RequiredBy { get; set; }
        public double PlannedCost { get; set; }
        public double ActualCost { get; set; }
        public double PlannedWork { get; set; }
        public double ActualWork { get; set; }
        public string UserInput { get; set; }
        public DateTime? FirstAssignedDate { get; set; }
        public DateTime? FirstResponseDate { get; set; }

        public WorkItemDto() : base()
        {

        }

        public WorkItemDto(IDictionary<string, object> dictionary) : base(dictionary)
        {

        }

        public WorkItemDto(ExpandableObject expandableObject) : base(expandableObject)
        {

        }
    }
}
