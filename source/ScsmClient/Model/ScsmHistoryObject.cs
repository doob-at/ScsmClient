using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doob.ScsmClient.Model
{
    public class ScsmHistoryObject
    {
        public Guid ScsmObjectId { get; set; }
        public HistoryTransaction[] Transactions { get; set; } = Array.Empty<HistoryTransaction>();

    }

    public class HistoryTransaction
    {
        public DateTime TimeStamp { get; set; }
        public string Username { get; set; }

        public PropertyChangeEntry[] PropertyChangeEntries { get; set; } = Array.Empty<PropertyChangeEntry>();

        public RelationshipChangeEntry[] RelationshipChangeEntries { get; set; } =
            Array.Empty<RelationshipChangeEntry>();
    }

    public class RelationshipChangeEntry
    {
        public string RelationshipClassName { get; set; }
        public string Action { get; set; }

        public object RelatedObject { get; set; }
    }

    public class PropertyChangeEntry
    {
        public string PropertyName { get; set; }
        public string Action { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }

}
