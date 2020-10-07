using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.Caches;

namespace ScsmClient.Operations
{
    public class EnumerationOperations : BaseOperation
    {
        //private static readonly ManagementPackElementCache<ManagementPackClass> CachedClasses = new ManagementPackElementCache<ManagementPackClass>();

        public EnumerationOperations(SCSMClient client) : base(client)
        {
        }


        public ManagementPackEnumeration GetEnumerationChildByName(ManagementPackEnumeration managementPackEnumeration, string name)
        {
            return GetEnumerationChildByName(managementPackEnumeration.Id, name, false) ??
                   GetEnumerationChildByDisplayName(managementPackEnumeration.Id, name, false) ??
                   GetEnumerationChildByName(managementPackEnumeration.Id, name, true) ??
                   GetEnumerationChildByDisplayName(managementPackEnumeration.Id, name, true);
        }

        public ManagementPackEnumeration GetEnumerationChildById(ManagementPackEnumeration managementPackEnumeration, Guid id)
        {
            return _client.ManagementGroup.EntityTypes
                .GetChildEnumerations(managementPackEnumeration.Id, TraversalDepth.Recursive)
                .FirstOrDefault(m => m.Id == id);

        }


        private ManagementPackEnumeration GetEnumerationChildByDisplayName(Guid id, string displayName, bool ignoreCase)
        {
            var stringComparsion = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return _client.ManagementGroup.EntityTypes
                .GetChildEnumerations(id, TraversalDepth.Recursive)
                .FirstOrDefault(m => m.DisplayName.Equals(displayName, stringComparsion));
        }

        private ManagementPackEnumeration GetEnumerationChildByName(Guid id, string name, bool ignoreCase)
        {
            var stringComparsion = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return _client.ManagementGroup.EntityTypes
                .GetChildEnumerations(id, TraversalDepth.Recursive)
                .FirstOrDefault(m => m.Name.Equals(name, stringComparsion));
        }


    }
}
