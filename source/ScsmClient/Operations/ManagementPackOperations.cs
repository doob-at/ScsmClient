using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using doob.Reflectensions.Common;
using doob.Reflectensions.Common.Helper;
using Microsoft.EnterpriseManagement.Configuration;


namespace ScsmClient.Operations
{
    public class ManagementPackOperations: BaseOperation
    {
       private readonly ConcurrentDictionary<Guid, ManagementPack> mpCache = new ConcurrentDictionary<Guid, ManagementPack>();

        internal ManagementPackOperations(SCSMClient client): base(client)
        {
            
        }

        public ManagementPack GetWellKnownManagementPack(string systemManagementPack)
        {

            if (Enum<SystemManagementPack>.TryFind(systemManagementPack, true, out var enu))
            {
                return _client.ManagementGroup.ManagementPacks.GetManagementPack(enu);
            }
            throw new ArgumentOutOfRangeException(nameof(systemManagementPack));
        }

        public ManagementPack GetWellKnownManagementPack(SystemManagementPack systemManagementPack)
        {
            return _client.ManagementGroup.ManagementPacks.GetManagementPack(systemManagementPack);
        }

        public ManagementPack GetManagementPackById(Guid id)
        {
            return mpCache.GetOrAdd(id,
                guid => _client.ManagementGroup.ManagementPacks.GetManagementPack(id));

        }

        public ManagementPack GetManagementPackById(string id)
        {
            return GetManagementPackById(id.ToGuid());
        }

        public ManagementPack GetManagementPackByName(string name, string keyToken, Version version)
        {
            return _client.ManagementGroup.ManagementPacks.GetManagementPack(name, keyToken, version);
        }

        public ManagementPack GetManagementPackByName(string name, string keyToken, string version)
        {
            var ver = Version.Parse(version);
            return _client.ManagementGroup.ManagementPacks.GetManagementPack(name, keyToken, ver);
        }

        public ManagementPack GetManagementPackByName(string name, Version version)
        {
            return GetManagementPackByName(name, null, version);
        }

        public ManagementPack GetManagementPackByName(string name, string version)
        {
            return GetManagementPackByName(name, null, version);

        }

        public ManagementPack GetManagementPackByName(string name)
        {
            var crit = new ManagementPackCriteria($"Name='{name}'");
            return _client.ManagementGroup.ManagementPacks.GetManagementPacks(crit).FirstOrDefault();

        }

        public IList<ManagementPack> GetManagementPacks()
        {
            return _client.ManagementGroup.ManagementPacks.GetManagementPacks();

        }
    }
}
