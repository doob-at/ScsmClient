using System;
using System.Collections.Concurrent;
using Microsoft.EnterpriseManagement.Configuration;
using Reflectensions.HelperClasses;

namespace BaseIT.SCSM.Client.Operations
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

        public ManagementPack GetManagementPack(Guid managementPackId)
        {
            return mpCache.GetOrAdd(managementPackId,
                guid => _client.ManagementGroup.ManagementPacks.GetManagementPack(managementPackId));

        }

        public ManagementPack GetManagementPack(string id)
        {
            return GetManagementPack(new Guid(id));
        }

        public ManagementPack GetManagementPack(string name, string keyToken, Version version)
        {
            return _client.ManagementGroup.ManagementPacks.GetManagementPack(name, keyToken, version);
        }

        public ManagementPack GetManagementPack(string name, string keyToken, string version)
        {
            var ver = Version.Parse(version);
            return _client.ManagementGroup.ManagementPacks.GetManagementPack(name, keyToken, ver);
        }

        public ManagementPack GetManagementPack(string name, Version version)
        {
            return GetManagementPack(name, null, version);
        }

        public ManagementPack GetManagementPack(string name, string version)
        {
            return GetManagementPack(name, null, version);

        }
    }
}
