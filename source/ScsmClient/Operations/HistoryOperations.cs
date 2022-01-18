using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using doob.Reflectensions.Common;
using doob.ScsmClient.Model;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;

using ScsmClient.Caches;

namespace ScsmClient.Operations
{
    public class HistoryOperations : BaseOperation
    {
        

        public HistoryOperations(SCSMClient client): base(client)
        {
            
        }


        public ScsmHistoryObject GetHistoryFromGenericId(Guid id)
        {
            var obj = _client.Object().GetEnterpriseManagementObjectById(id);
            var transactions = _client.ManagementGroup.EntityObjects.GetObjectHistoryTransactions(obj);

            var histObject = new ScsmHistoryObject();
            histObject.ScsmObjectId = id;
            var transList = new List<HistoryTransaction>();
            

            foreach (var transaction in transactions)
            {
                var trans = new HistoryTransaction();
                trans.TimeStamp = transaction.DateOccurred;
                trans.Username = transaction.UserName;
                var classChanges = new List<PropertyChangeEntry>();
                var relationShipChanges = new List<RelationshipChangeEntry>();

                foreach (var hist in transaction.ObjectHistory)
                {

                    foreach (var classHistory in hist.Value.ClassHistory)
                    {


                        foreach (var classHistoryPropertyChange in classHistory.PropertyChanges)
                        {
                            var entry = new PropertyChangeEntry();
                            entry.PropertyName = classHistoryPropertyChange.Key.DisplayName.TrimToNull() ?? classHistoryPropertyChange.Key.Name;
                            entry.Action = classHistory.ChangeType.GetName();
                            entry.OldValue = NormalizeValue(classHistoryPropertyChange.Value.First.Value);
                            entry.NewValue = NormalizeValue(classHistoryPropertyChange.Value.Second.Value);
                            classChanges.Add(entry);
                        }
                    }

                    foreach (var relationshipHistory in hist.Value.RelationshipHistory)
                    {
                        var relationShip = _client.Types().GetRelationShipById(relationshipHistory.ManagementPackRelationshipTypeId);
                        var entry = new RelationshipChangeEntry();
                        entry.RelationshipClassName = relationShip.DisplayName.TrimToNull() ?? relationShip.Name.TrimToNull();
                        entry.Action = relationshipHistory.ChangeType.GetName();

                        var relId = relationshipHistory.SourceObjectId != id
                            ? relationshipHistory.SourceObjectId
                            : relationshipHistory.TargetObjectId;

                        var related = _client.ScsmObject().GetObjectById(relId);
                        var r = new Dictionary<string, string>();
                        r["Id"] = related.ObjectId.ToString();
                        r["DisplayName"] = related.DisplayName;
                        entry.RelatedObject = r;
                        relationShipChanges.Add(entry);
                    }
                }

                trans.PropertyChangeEntries = classChanges.ToArray();
                trans.RelationshipChangeEntries = relationShipChanges.ToArray();
                transList.Add(trans);
            }


            histObject.Transactions = transList.ToArray();


            return histObject;
        }


        private static object NormalizeValue(object value)
        {
            if (value is ManagementPackEnumeration en)
            {
                return en.DisplayName;
            }

            return value;
        }
    }
}
