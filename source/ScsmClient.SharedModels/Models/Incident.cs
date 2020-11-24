using System;
using System.Collections.Generic;
using Reflectensions.HelperClasses;

namespace ScsmClient.SharedModels.Models
{
    public class Incident: TroubleTicket
    {
        private DateTime? _targetResolutionTime;
        private bool _escalated;
        private string _source;
        private string _status;
        private string _resolutionDescription;
        private bool _needsKnowledgeArticle;
        private string _tierQueue;
        private bool _hasCreatedKnowledgeArticle;
        private string _lastModifiedSource;
        private string _classification;
        private string _resolutionCategory;

        public DateTime? TargetResolutionTime
        {
            get => _targetResolutionTime;
            set => SetPropertyChanged(ref _targetResolutionTime, value);
        }

        public bool Escalated
        {
            get => _escalated;
            set => SetPropertyChanged(ref _escalated, value);
        }

        public string Source
        {
            get => _source;
            set => SetPropertyChanged(ref _source, value);
        }

        public string Status
        {
            get => _status;
            set => SetPropertyChanged(ref _status, value);
        }

        public string ResolutionDescription
        {
            get => _resolutionDescription;
            set => SetPropertyChanged(ref _resolutionDescription, value);
        }

        public bool NeedsKnowledgeArticle
        {
            get => _needsKnowledgeArticle;
            set => SetPropertyChanged(ref _needsKnowledgeArticle, value);
        }

        public string TierQueue
        {
            get => _tierQueue;
            set => SetPropertyChanged(ref _tierQueue, value);
        }

        public bool HasCreatedKnowledgeArticle
        {
            get => _hasCreatedKnowledgeArticle;
            set => SetPropertyChanged(ref _hasCreatedKnowledgeArticle, value);
        }

        public string LastModifiedSource
        {
            get => _lastModifiedSource;
            set => SetPropertyChanged(ref _lastModifiedSource, value);
        }

        public string Classification
        {
            get => _classification;
            set => SetPropertyChanged(ref _classification, value);
        }

        public string ResolutionCategory
        {
            get => _resolutionCategory;
            set => SetPropertyChanged(ref _resolutionCategory, value);
        }

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
