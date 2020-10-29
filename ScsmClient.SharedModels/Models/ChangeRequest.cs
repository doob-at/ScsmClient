using System;
using System.Collections.Generic;
using Reflectensions.HelperClasses;


namespace ScsmClient.SharedModels.Models
{
    public class ChangeRequest: WorkItem
    {
        public string Reason { get; set; }
        public string Notes { get; set; }
        public string ImplementationPlan { get; set; }
        public string RiskAssessmentPlan { get; set; }
        public string BackoutPlan { get; set; }
        public string TestPlan { get; set; }
        public string PostImplementationReview { get; set; }
        public string TemplateId { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Impact { get; set; }
        public string Risk { get; set; }
        public string ImplementationResults { get; set; }
        public string Area { get; set; }

        public ChangeRequest() : base()
        {

        }

        public ChangeRequest(IDictionary<string, object> dictionary) : base(dictionary)
        {

        }

        public ChangeRequest(ExpandableObject expandableObject) : base(expandableObject)
        {

        }
    }
}
