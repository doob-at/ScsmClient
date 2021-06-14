using System;
using System.Collections.Generic;
using doob.Reflectensions;

namespace ScsmClient.SharedModels.Models
{
    
    public class ScsmObject : ExpandableObject
    {
        private Guid _objectId;
        public Guid ObjectId
        {
            get => _objectId;
            set => SetPropertyChanged(ref _objectId, value);
        }

        private DateTime _lastModified;
        public DateTime LastModified
        {
            get => _lastModified;
            set => SetPropertyChanged(ref _lastModified, value);
        }

        private DateTime _timeAdded;
        public DateTime TimeAdded
        {
            get => _timeAdded;
            set => SetPropertyChanged(ref _timeAdded, value);
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => SetPropertyChanged(ref _displayName, value);
        }

        public ScsmObject(): base()
        {
            
        }

        public ScsmObject(IDictionary<string, object> dictionary) : base(dictionary)
        {

        }

        public ScsmObject(ExpandableObject expandableObject) : base(expandableObject)
        {

        }
    }

}
