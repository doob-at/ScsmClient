using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;
using Reflectensions.ExtensionMethods;
using ScsmClient.Caches;

namespace ScsmClient.Operations
{
    public class TypeOperations: BaseOperation
    {
        private static readonly ManagementPackElementCache<ManagementPackClass> CachedClasses = new ManagementPackElementCache<ManagementPackClass>();
        private static readonly ManagementPackElementCache<ManagementPackTypeProjection> CachedTypeProjections = new ManagementPackElementCache<ManagementPackTypeProjection>();
        private static readonly ManagementPackElementCache<ManagementPackRelationship> CachedRelationShips = new ManagementPackElementCache<ManagementPackRelationship>();

        public TypeOperations(SCSMClient client) : base(client)
        {
        }


        #region ManagementPackClass
        public ManagementPackClass GetClassById(string id)
        {
            return GetClassById(id.ToGuid());
        }
        
        public ManagementPackClass GetClassById(Guid id)
        {
            return CachedClasses.GetOrAddById(id, s =>
            {
                try
                {
                    return _client.ManagementGroup.EntityTypes.GetClass(id);
                }
                catch (Exception e)
                {
                    return null;
                }
                
            });

        }
        
        public ManagementPackClass GetClassByName(string name)
        {
            return CachedClasses.GetOrAddByName(name, s =>
            {
                var parsed = ParseName(name);

                if (String.IsNullOrEmpty(parsed.ManagementPackName))
                {
                    var crit = new ManagementPackClassCriteria($"Name='{name}'");
                    return GetClassesByCriteria(crit).FirstOrDefault();
                }
                else
                {
                    var mp = _client.ManagementPack().GetManagementPackByName(parsed.ManagementPackName);
                    return GetClassByName(parsed.TypeName, mp);
                }
            });

        }
        
        public ManagementPackClass GetClassByName(string name, ManagementPack managementPack)
        {

            return CachedClasses.GetOrAddByName($"[{managementPack.Name}]{name}", s =>
            {
                try
                {
                    var parsed = ParseName(name);
                    return _client.ManagementGroup.EntityTypes.GetClass(parsed.TypeName, managementPack);
                }
                catch (Exception e)
                {
                    return null;
                }
                
            });

        }
        
        public IList<ManagementPackClass> GetClassesByCriteria(ManagementPackClassCriteria criteria)
        {
            return _client.ManagementGroup.EntityTypes.GetClasses(criteria);
        }

        public IList<ManagementPackClass> GetClassesByCriteria(string criteria)
        {
            return _client.ManagementGroup.EntityTypes.GetClasses(new ManagementPackClassCriteria(criteria));
        }
        #endregion


        #region TypeProjection
        public ManagementPackTypeProjection GetTypeProjectionById(string id)
        {
            return GetTypeProjectionById(id.ToGuid());
        }

        public ManagementPackTypeProjection GetTypeProjectionById(Guid id)
        {
            return CachedTypeProjections.GetOrAddById(id, s =>
            {
                return _client.ManagementGroup.EntityTypes.GetTypeProjection(id);
            });

        }

        public ManagementPackTypeProjection GetTypeProjectionByName(string name)
        {
            return CachedTypeProjections.GetOrAddByName(name, s =>
            {
                var parsed = ParseName(name);

                if (String.IsNullOrEmpty(parsed.ManagementPackName))
                {
                    var crit = new ManagementPackTypeProjectionCriteria($"Name='{name}'");
                    return GetTypeProjectionsByCriteria(crit).FirstOrDefault();
                }
                else
                {
                    var mp = _client.ManagementPack().GetManagementPackByName(parsed.ManagementPackName);
                    return GetTypeProjectionByName(parsed.TypeName, mp);
                }
            });

        }

        public ManagementPackTypeProjection GetTypeProjectionByName(string name, ManagementPack managementPack)
        {
            return CachedTypeProjections.GetOrAddByName(name, s =>
            {
                var parsed = ParseName(name);
                return _client.ManagementGroup.EntityTypes.GetTypeProjection(parsed.TypeName, managementPack);
            });
        }

        public IList<ManagementPackTypeProjection> GetTypeProjectionsByCriteria(string criteria)
        {
            var crit = new ManagementPackTypeProjectionCriteria(criteria);
            return GetTypeProjectionsByCriteria(crit);
        }
        public IList<ManagementPackTypeProjection> GetTypeProjectionsByCriteria(ManagementPackTypeProjectionCriteria criteria)
        {
            return _client.ManagementGroup.EntityTypes.GetTypeProjections(criteria);
        }
        public IList<ManagementPackTypeProjection> GetTypeProjections()
        {
            return _client.ManagementGroup.EntityTypes.GetTypeProjections();
        }
        #endregion


        #region RelationShip

        public ManagementPackRelationship GetRelationShipById(string id)
        {
            return GetRelationShipById(id.ToGuid());
        }

        public ManagementPackRelationship GetRelationShipById(Guid id)
        {
            return CachedRelationShips.GetOrAddById(id, s =>
            {
                return _client.ManagementGroup.EntityTypes.GetRelationshipClass(id);
            });

        }

        public ManagementPackRelationship GetRelationshipByName(string name)
        {
            return CachedRelationShips.GetOrAddByName(name, s =>
            {
                var parsed = ParseName(name);

                if (String.IsNullOrEmpty(parsed.ManagementPackName))
                {
                    var crit = new ManagementPackRelationshipCriteria($"Name='{name}'");
                    return GetRelationshipByCriteria(crit).FirstOrDefault();
                }
                else
                {
                    var mp = _client.ManagementPack().GetManagementPackByName(parsed.ManagementPackName);
                    return GetRelationshipByName(parsed.TypeName, mp);
                }
            });

        }

        public ManagementPackRelationship GetRelationshipByName(string name, ManagementPack managementPack)
        {
            return CachedRelationShips.GetOrAddByName(name, s =>
            {
                var parsed = ParseName(name);
                var crit = new ManagementPackRelationshipCriteria($"Name='{parsed.TypeName}'");
                return GetRelationshipByCriteria(crit).FirstOrDefault(r => r.GetManagementPack().Id == managementPack.Id);
            });
        }

        public IList<ManagementPackRelationship> GetRelationshipByCriteria(ManagementPackRelationshipCriteria criteria)
        {
            return _client.ManagementGroup.EntityTypes.GetRelationshipClasses();
        }


        public ManagementPackRelationship GetRelationshipClassByName(string name)
        {
            var crit = new ManagementPackRelationshipCriteria($"Name='{name}'");
            return _client.ManagementGroup.EntityTypes.GetRelationshipClasses(crit).FirstOrDefault();
        }

        #endregion



        private (string TypeName, string ManagementPackName) ParseName(string name)
        {

            var regMatch = new Regex(@"(\[(?<managementPack>.+)\])?(?<name>.+)").Match(name);

            var managementPack = regMatch.Groups["managementPack"]?.Value;
            var _name = regMatch.Groups["name"]?.Value;

            return (_name, managementPack);
        }

    }
}
