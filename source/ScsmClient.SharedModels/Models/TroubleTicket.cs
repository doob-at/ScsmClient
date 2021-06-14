using System;
using System.Collections.Generic;
using doob.Reflectensions;

namespace ScsmClient.SharedModels.Models
{
    public class TroubleTicket : WorkItem
    {
        private int _priority;
        private string _urgency;
        private string _impact;
        private DateTime? _closedDate;
        private DateTime? _resolvedDate;

        public int Priority
        {
            get => _priority;
            set => SetPropertyChanged(ref _priority, value);
        }

        public string Urgency
        {
            get => _urgency;
            set => SetPropertyChanged(ref _urgency, value);
        }

        public string Impact
        {
            get => _impact;
            set => SetPropertyChanged(ref _impact, value);
        }

        public DateTime? ClosedDate
        {
            get => _closedDate;
            set => SetPropertyChanged(ref _closedDate, value);
        }

        public DateTime? ResolvedDate
        {
            get => _resolvedDate;
            set => SetPropertyChanged(ref _resolvedDate, value);
        }

        protected TroubleTicket() : base()
        {

        }

        protected TroubleTicket(IDictionary<string, object> dictionary) : base(dictionary)
        {

        }

        protected TroubleTicket(ExpandableObject expandableObject) : base(expandableObject)
        {

        }
    }
}