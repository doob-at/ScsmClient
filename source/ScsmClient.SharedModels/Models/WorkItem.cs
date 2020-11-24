using System;
using System.Collections.Generic;
using Reflectensions.HelperClasses;

namespace ScsmClient.SharedModels.Models
{
    public class WorkItem: ScsmObject
    {
        private string _title;
        private string _description;
        private string _contactMethod;
        private DateTime? _createdDate;
        private DateTime? _scheduledStartDate;
        private DateTime? _scheduledEndDate;
        private DateTime? _actualStartDate;
        private DateTime? _actualEndDate;
        private bool _isDowntime;
        private bool _isParent;
        private DateTime? _scheduledDowntimeStartDate;
        private DateTime? _scheduledDowntimeEndDate;
        private DateTime? _actualDowntimeStartDate;
        private DateTime? _actualDowntimeEndDate;
        private DateTime? _requiredBy;
        private double _plannedCost;
        private double _actualCost;
        private double _plannedWork;
        private double _actualWork;
        private UserInput _userInput;
        private DateTime? _firstAssignedDate;
        private DateTime? _firstResponseDate;

        public string Title
        {
            get => _title;
            set => SetPropertyChanged(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetPropertyChanged(ref _description, value);
        }

        public string ContactMethod
        {
            get => _contactMethod;
            set => SetPropertyChanged(ref _contactMethod, value);
        }

        public DateTime? CreatedDate
        {
            get => _createdDate;
            set => SetPropertyChanged(ref _createdDate, value);
        }

        public DateTime? ScheduledStartDate
        {
            get => _scheduledStartDate;
            set => SetPropertyChanged(ref _scheduledStartDate, value);
        }

        public DateTime? ScheduledEndDate
        {
            get => _scheduledEndDate;
            set => SetPropertyChanged(ref _scheduledEndDate, value);
        }

        public DateTime? ActualStartDate
        {
            get => _actualStartDate;
            set => SetPropertyChanged(ref _actualStartDate, value);
        }

        public DateTime? ActualEndDate
        {
            get => _actualEndDate;
            set => SetPropertyChanged(ref _actualEndDate, value);
        }

        public bool IsDowntime
        {
            get => _isDowntime;
            set => SetPropertyChanged(ref _isDowntime, value);
        }

        public bool IsParent
        {
            get => _isParent;
            set => SetPropertyChanged(ref _isParent, value);
        }

        public DateTime? ScheduledDowntimeStartDate
        {
            get => _scheduledDowntimeStartDate;
            set => SetPropertyChanged(ref _scheduledDowntimeStartDate, value);
        }

        public DateTime? ScheduledDowntimeEndDate
        {
            get => _scheduledDowntimeEndDate;
            set => SetPropertyChanged(ref _scheduledDowntimeEndDate, value);
        }

        public DateTime? ActualDowntimeStartDate
        {
            get => _actualDowntimeStartDate;
            set => SetPropertyChanged(ref _actualDowntimeStartDate, value);
        }

        public DateTime? ActualDowntimeEndDate
        {
            get => _actualDowntimeEndDate;
            set => SetPropertyChanged(ref _actualDowntimeEndDate, value);
        }

        public DateTime? RequiredBy
        {
            get => _requiredBy;
            set => SetPropertyChanged(ref _requiredBy, value);
        }

        public double PlannedCost
        {
            get => _plannedCost;
            set => SetPropertyChanged(ref _plannedCost, value);
        }

        public double ActualCost
        {
            get => _actualCost;
            set => SetPropertyChanged(ref _actualCost, value);
        }

        public double PlannedWork
        {
            get => _plannedWork;
            set => SetPropertyChanged(ref _plannedWork, value);
        }

        public double ActualWork
        {
            get => _actualWork;
            set => SetPropertyChanged(ref _actualWork, value);
        }

        public UserInput UserInput
        {
            get => _userInput;
            set => SetPropertyChanged(ref _userInput, value);
        }

        public DateTime? FirstAssignedDate
        {
            get => _firstAssignedDate;
            set => SetPropertyChanged(ref _firstAssignedDate, value);
        }

        public DateTime? FirstResponseDate
        {
            get => _firstResponseDate;
            set => SetPropertyChanged(ref _firstResponseDate, value);
        }

        public WorkItem() : base()
        {

        }

        public WorkItem(IDictionary<string, object> dictionary) : base(dictionary)
        {

        }

        public WorkItem(ExpandableObject expandableObject) : base(expandableObject)
        {

        }
    }
}
