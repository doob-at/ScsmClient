using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Common;

namespace ScsmClient.Caches
{
    internal class EnterpriseManagementObjectCache<T> where T: EnterpriseManagementObject
    {
        private Dictionary<string, T> CacheByName = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<Guid, T> CacheById = new Dictionary<Guid, T>();

        internal T GetOrAddByName(string name, Func<string, T> update)
        {
            if (CacheByName.TryGetValue(name, out var item))
            {
                return item;
            }

            item = update(name);
            CacheByName[item.Name] = item;
            CacheById[item.Id] = item;
            return item;

        }

        internal T GetOrAddById(Guid id, Func<Guid, T> update)
        {
            if (CacheById.TryGetValue(id, out var item))
            {
                return item;
            }

            item = update(id);
            CacheByName[item.Name] = item;
            CacheById[item.Id] = item;
            return item;
        }
    }
}
