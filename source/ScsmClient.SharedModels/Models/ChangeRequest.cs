using System;
using System.Collections.Generic;
using Reflectensions.HelperClasses;


namespace ScsmClient.SharedModels.Models
{
    public class ChangeRequest: WorkItem
    {
        private string _reason;
        private string _notes;
        private string _implementationPlan;
        private string _riskAssessmentPlan;
        private string _backoutPlan;
        private string _testPlan;
        private string _postImplementationReview;
        private string _templateId;
        private DateTime? _requiredByDate;
        private string _status;
        private string _category;
        private string _priority;
        private string _impact;
        private string _risk;
        private string _implementationResults;
        private string _area;

        public string Reason
        {
            get => _reason;
            set => SetPropertyChanged(ref _reason, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetPropertyChanged(ref _notes, value);
        }

        public string ImplementationPlan
        {
            get => _implementationPlan;
            set => SetPropertyChanged(ref _implementationPlan, value);
        }

        public string RiskAssessmentPlan
        {
            get => _riskAssessmentPlan;
            set => SetPropertyChanged(ref _riskAssessmentPlan, value);
        }

        public string BackoutPlan
        {
            get => _backoutPlan;
            set => SetPropertyChanged(ref _backoutPlan, value);
        }

        public string TestPlan
        {
            get => _testPlan;
            set => SetPropertyChanged(ref _testPlan, value);
        }

        public string PostImplementationReview
        {
            get => _postImplementationReview;
            set => SetPropertyChanged(ref _postImplementationReview, value);
        }

        public string TemplateId
        {
            get => _templateId;
            set => SetPropertyChanged(ref _templateId, value);
        }

        public DateTime? RequiredByDate
        {
            get => _requiredByDate;
            set => SetPropertyChanged(ref _requiredByDate, value);
        }

        public string Status
        {
            get => _status;
            set => SetPropertyChanged(ref _status, value);
        }

        public string Category
        {
            get => _category;
            set => SetPropertyChanged(ref _category, value);
        }

        public string Priority
        {
            get => _priority;
            set => SetPropertyChanged(ref _priority, value);
        }

        public string Impact
        {
            get => _impact;
            set => SetPropertyChanged(ref _impact, value);
        }

        public string Risk
        {
            get => _risk;
            set => SetPropertyChanged(ref _risk, value);
        }

        public string ImplementationResults
        {
            get => _implementationResults;
            set => SetPropertyChanged(ref _implementationResults, value);
        }

        public string Area
        {
            get => _area;
            set => SetPropertyChanged(ref _area, value);
        }

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
