using System;
using System.Collections.Generic;
using System.Text;
using Reflectensions.HelperClasses;

namespace ScsmClient.SharedModels.Models
{
    public class Activity: WorkItem
    {
        private int _sequenceId;
        private string _childId;
        private string _notes;
        private string _status;
        private string _priority;
        private string _area;
        private string _stage;
        private string _documentation;
        private bool _skip;

        public int SequenceId
        {
            get => _sequenceId;
            set => SetPropertyChanged(ref _sequenceId, value);
        }

        public string ChildId
        {
            get => _childId;
            set => SetPropertyChanged(ref _childId, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetPropertyChanged(ref _notes, value);
        }

        public string Status
        {
            get => _status;
            set => SetPropertyChanged(ref _status, value);
        }

        public string Priority
        {
            get => _priority;
            set => SetPropertyChanged(ref _priority, value);
        }

        public string Area
        {
            get => _area;
            set => SetPropertyChanged(ref _area, value);
        }

        public string Stage
        {
            get => _stage;
            set => SetPropertyChanged(ref _stage, value);
        }

        public string Documentation
        {
            get => _documentation;
            set => SetPropertyChanged(ref _documentation, value);
        }

        public bool Skip
        {
            get => _skip;
            set => SetPropertyChanged(ref _skip, value);
        }

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
