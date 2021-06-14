using System;
using System.Collections.Generic;
using doob.Reflectensions;

namespace ScsmClient.SharedModels.Models
{
    public class ServiceRequest: WorkItem
    {
        private string _status;
        private string _templateId;
        private string _priority;
        private string _urgency;
        private DateTime? _completedDate;
        private DateTime? _closedDate;
        private string _source;
        private string _implementationResults;
        private string _notes;
        private string _area;
        private string _supportGroup;

        public string Status
        {
            get => _status;
            set => SetPropertyChanged(ref _status, value);
        }

        public string TemplateId
        {
            get => _templateId;
            set => SetPropertyChanged(ref _templateId, value);
        }

        public string Priority
        {
            get => _priority;
            set => SetPropertyChanged(ref _priority, value);
        }

        public string Urgency
        {
            get => _urgency;
            set => SetPropertyChanged(ref _urgency, value);
        }

        public DateTime? CompletedDate
        {
            get => _completedDate;
            set => SetPropertyChanged(ref _completedDate, value);
        }

        public DateTime? ClosedDate
        {
            get => _closedDate;
            set => SetPropertyChanged(ref _closedDate, value);
        }

        public string Source
        {
            get => _source;
            set => SetPropertyChanged(ref _source, value);
        }

        public string ImplementationResults
        {
            get => _implementationResults;
            set => SetPropertyChanged(ref _implementationResults, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetPropertyChanged(ref _notes, value);
        }

        public string Area
        {
            get => _area;
            set => SetPropertyChanged(ref _area, value);
        }

        public string SupportGroup
        {
            get => _supportGroup;
            set => SetPropertyChanged(ref _supportGroup, value);
        }

        public ServiceRequest() : base()
        {

        }

        public ServiceRequest(IDictionary<string, object> dictionary): base(dictionary)
        {
            
        }

        public ServiceRequest(ExpandableObject expandableObject) : base(expandableObject)
        {

        }
    }
}
