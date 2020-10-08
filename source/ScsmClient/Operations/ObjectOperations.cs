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
using ScsmClient.Helper;
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

        public IEnumerable<EnterpriseManagementObjectDto> GetObjectsByClassName(string className, string criteria, int? maxResult = null)
        {
            var objectClass = _client.Class().GetClassByName(className);
            return GetObjectsByClass(objectClass, criteria, maxResult);
        }

        public IEnumerable<EnterpriseManagementObjectDto> GetObjectsByClassId(Guid classId, string criteria, int? maxResult = null)
        {
            var objectClass = _client.Class().GetClassById(classId);
            return GetObjectsByClass(objectClass, criteria, maxResult);
        }

        public IEnumerable<EnterpriseManagementObjectDto> GetObjectsByClass(ManagementPackClass objectClass, string criteria, int? maxResult = null)
        {

           
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
        

        public EnterpriseManagementObjectDto CreateObjectByClassId(Guid id, Dictionary<string, object> properties)
        {
            var objectClass = _client.Class().GetClassById(id);
            return CreateObjectByClass(objectClass, properties);
        }

        public EnterpriseManagementObjectDto CreateObjectByClassName(string className, Dictionary<string, object> properties)
        {
            var objectClass = _client.Class().GetClassByName(className);
            return CreateObjectByClass(objectClass, properties);
        }

        public EnterpriseManagementObjectDto CreateObjectByClass(ManagementPackClass objectClass, Dictionary<string, object> properties)
        {
            var obj = new CreatableEnterpriseManagementObject(_client.ManagementGroup, objectClass);
            var objectProperties = objectClass.GetProperties(BaseClassTraversalDepth.Recursive);
            var normalizer = new ValueConverter(_client);

            foreach (var kv in properties)
            {
                var prop = objectProperties.FirstOrDefault(p => p.Name.Equals(kv.Key, StringComparison.Ordinal)) ??
                           objectProperties.FirstOrDefault(p => p.Name.Equals(kv.Key, StringComparison.OrdinalIgnoreCase));
                if (prop != null)
                {
                    
                    var val = normalizer.NormalizeValue(kv.Value, prop);
                    obj[objectClass, kv.Key].Value = val;
                }
            }

            obj.Commit();
            return GetObjectById(obj.Id);
        }


        public EnterpriseManagementObjectDto CreateObjectFromTemplateName(string templateName, Dictionary<string, object> properties)
        {

            var template =_client.Template().GetObjectTemplateByName(templateName);
            return CreateObjectFromTemplate(template, properties);
        }

        public EnterpriseManagementObjectDto CreateObjectFromTemplate(ManagementPackObjectTemplate template, Dictionary<string, object> properties)
        {

            var obj = new EnterpriseManagementObjectProjection(_client.ManagementGroup, template);
            var normalizer = new ValueConverter(_client);

            var elem = template.TypeID.GetElement();
            if (elem is ManagementPackTypeProjection managementPackTypeProjection)
            {
                var objectProperties = managementPackTypeProjection.TargetType.GetProperties(BaseClassTraversalDepth.Recursive);
                foreach (var kv in properties)
                {
                    var prop = objectProperties.FirstOrDefault(p => p.Name.Equals(kv.Key, StringComparison.Ordinal)) ??
                               objectProperties.FirstOrDefault(p => p.Name.Equals(kv.Key, StringComparison.OrdinalIgnoreCase));

                    var val = normalizer.NormalizeValue(kv.Value, prop);
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
