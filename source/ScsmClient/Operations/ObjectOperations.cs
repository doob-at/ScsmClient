using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.ExtensionMethods;
using ScsmClient.Model;

namespace ScsmClient.Operations
{
    public class ObjectOperations: BaseOperation
    {
        public ObjectOperations(SCSMClient client) : base(client)
        {
        }

        public IEnumerable<EnterpriseManagementObjectDto> GetObject(string className, string criteria, int? maxResult = null)
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


        public void CreateObject(string className, Dictionary<string, object> properties)
        {
            var objectClass = _client.Class().GetClassByName(className);
            
            var obj = new CreatableEnterpriseManagementObject(_client.ManagementGroup, objectClass);

            foreach (var kv in properties)
            {
                obj[objectClass, kv.Key].Value = kv.Value;
            }

            obj.Commit();
            
        }

        public void CreateObjectFromTemplate(string templateName, Dictionary<string, object> properties)
        {

            var template =_client.Template().GetObjectTemplateByName(templateName);
            CreateObjectFromTemplate(template, properties);
        }

        public void CreateObjectFromTemplate(ManagementPackObjectTemplate template, Dictionary<string, object> properties)
        {

            var obj = new CreatableEnterpriseManagementObject(_client.ManagementGroup, template);
            var objectClass = template.TypeConstraint.GetElement();
            foreach (var kv in properties)
            {
                obj[objectClass, kv.Key].Value = kv.Value;
            }

            obj.Commit();

        }

    }
}
