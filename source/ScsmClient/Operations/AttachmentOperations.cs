using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Reflectensions.ExtensionMethods;
using ScsmClient.Caches;

namespace ScsmClient.Operations
{
    public class AttachmentOperations : BaseOperation
    {
        
        public AttachmentOperations(SCSMClient client) : base(client)
        {
        }

        public Guid AddAttachment(Guid sourceId, string displayName, Stream content, string description)
        {
            var sourceObject = _client.Object().GetEnterpriseManagementObjectById(sourceId);
            return AddAttachment(sourceObject, displayName, content, description);
        }

        public Guid AddAttachment(EnterpriseManagementObject sourceObject, string displayName, Stream content, string description)
        {
            var fileAttachmentClass = _client.Types().GetClassByName("System.FileAttachment");
            var obj = new CreatableEnterpriseManagementObject(_client.ManagementGroup, fileAttachmentClass);

            obj[fileAttachmentClass, "Id"].Value = Guid.NewGuid().ToString();
            obj[fileAttachmentClass, "DisplayName"].Value = displayName;
            obj[fileAttachmentClass, "Content"].Value = content;
            obj[fileAttachmentClass, "Size"].Value = content.Length;
           
            obj[fileAttachmentClass, "AddedDate"].Value = DateTime.UtcNow;

            if (description != null)
            {
                obj[fileAttachmentClass, "Description"].Value = description;
            }
            


            if (displayName.Contains('.'))
            {
                obj[fileAttachmentClass, "Extension"].Value = displayName.Split('.').Last();
            }

            _client.Relations().CreateRelation(sourceObject, obj);
            return obj.Id;
        }


        public IEnumerable<EnterpriseManagementObject> GetAttachments(Guid sourceId)
        {
            return _client.Relations().GetRelatedObjectsByClassName(sourceId, "System.FileAttachment");
        }
    }
}
