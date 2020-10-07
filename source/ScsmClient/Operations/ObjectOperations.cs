using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Reflectensions.ExtensionMethods;
using ScsmClient.Attributes;
using ScsmClient.ExtensionMethods;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Operations
{
    public class ObjectOperations: BaseOperation
    {
        public ObjectOperations(SCSMClient client) : base(client)
        {
        }
        
        public EnterpriseManagementObjectDto GetObjectById(Guid id)
        {
            var critOptions = new ObjectQueryOptions();
            critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.NonBuffered;

            return _client.ManagementGroup.EntityObjects.GetObject<EnterpriseManagementObject>(id, critOptions).ToObjectDto();
        }

        public IEnumerable<EnterpriseManagementObjectDto> GetObjects(string className, string criteria, int? maxResult = null)
        {

            var objectClass = _client.Class().GetClassByName(className);
            var crit = _client.Criteria().BuildObjectCriteria(criteria, objectClass);


            var critOptions = new ObjectQueryOptions();
            critOptions.DefaultPropertyRetrievalBehavior = ObjectPropertyRetrievalBehavior.All;
            critOptions.ObjectRetrievalMode = ObjectRetrievalOptions.NonBuffered;
            critOptions.MaxResultCount = maxResult ?? Int32.MaxValue;


            var reader = _client.ManagementGroup.EntityObjects.GetObjectReader<EnterpriseManagementObject>(crit, critOptions);
            
            var count = 0;
            
            foreach (EnterpriseManagementObject enterpriseManagementObject in reader)
            {
                if (count == critOptions.MaxResultCount)
                    break;
                yield return enterpriseManagementObject.ToObjectDto();
            }

        }

        public EnterpriseManagementObjectDto CreateObject(Guid id, Dictionary<string, object> properties)
        {
            var objectClass = _client.Class().GetClassById(id);
            return CreateObject(objectClass, properties);
        }

        public EnterpriseManagementObjectDto CreateObject(string className, Dictionary<string, object> properties)
        {
            var objectClass = _client.Class().GetClassByName(className);
            return CreateObject(objectClass, properties);
        }

        public EnterpriseManagementObjectDto CreateObject(ManagementPackClass objectClass, Dictionary<string, object> properties)
        {
            var obj = new CreatableEnterpriseManagementObject(_client.ManagementGroup, objectClass);
            var objectProperties = objectClass.GetProperties(BaseClassTraversalDepth.Recursive);


            foreach (var kv in properties)
            {
                var prop = objectProperties.FirstOrDefault(p => p.Name.Equals(kv.Key, StringComparison.Ordinal)) ??
                           objectProperties.FirstOrDefault(p => p.Name.Equals(kv.Key, StringComparison.OrdinalIgnoreCase));
                if (prop != null)
                {
                    var val = NormalizeValue(kv, prop);
                    obj[objectClass, kv.Key].Value = val;
                }
            }

            obj.Commit();
            return GetObjectById(obj.Id);
        }

        private object NormalizeValue(KeyValuePair<string, object> keyValue, ManagementPackProperty property)
        {
            

            if (property.Type == ManagementPackEntityPropertyTypes.@enum)
            {
                return NormalizeEnum(keyValue.Value, property.EnumType.GetElement());
            }

            return keyValue.Value;
        }

        private object NormalizeEnum(object enumValue, ManagementPackEnumeration managementPackEnumeration)
        {
            if (enumValue is Guid guid)
            {
                return _client.Enumeration().GetEnumerationChildById(managementPackEnumeration, guid);
            }
            else if (enumValue is string str)
            {
                if (str.IsGuid())
                {
                    return NormalizeEnum(str.ToGuid(), managementPackEnumeration);
                }
                return _client.Enumeration().GetEnumerationChildByName(managementPackEnumeration, str);
            }
            else if (enumValue is Enum enu)
            {
                if (enu.HasId())
                {
                    return NormalizeEnum(enu.Id(), managementPackEnumeration);
                }
               
            }

            return enumValue;
        }

        public EnterpriseManagementObjectDto CreateObjectFromTemplate(string templateName, Dictionary<string, object> properties)
        {

            var template =_client.Template().GetObjectTemplateByName(templateName);
            return CreateObjectFromTemplate(template, properties);
        }

        public EnterpriseManagementObjectDto CreateObjectFromTemplate(ManagementPackObjectTemplate template, Dictionary<string, object> properties)
        {

            var obj = new EnterpriseManagementObjectProjection(_client.ManagementGroup, template);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                var objectProperties = managementPackTypeProjection.TargetType.GetProperties(BaseClassTraversalDepth.Recursive);
                foreach (var kv in properties)
                {
                    var prop = objectProperties.FirstOrDefault(p => p.Name.Equals(kv.Key, StringComparison.Ordinal)) ??
                               objectProperties.FirstOrDefault(p => p.Name.Equals(kv.Key, StringComparison.OrdinalIgnoreCase));

                    var val = NormalizeValue(kv, prop);
                    obj.Object[managementPackTypeProjection.TargetType, kv.Key].Value = val;
                }

                obj.Commit();


                return GetObjectById(obj.Object.Id);
            }
            else
            {
                throw new Exception($"Template '{template.DisplayName}' is invalid!");
            }
            

        }

    }
}
