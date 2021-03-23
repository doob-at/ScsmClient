using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;
using Reflectensions.ExtensionMethods;
using ScsmClient.Model;

namespace ScsmClient.ExtensionMethods
{
    public static class ManagementPackClassPropertyExtensions
    {
        public static ScsmClassProperty ToScsmClassProperty(this ManagementPackClassProperty managementPackClassProperty)
        {
            var p = new ScsmClassProperty();
            p.Id = managementPackClassProperty.Id;
            p.Name = managementPackClassProperty.Name;
            p.DisplayName = managementPackClassProperty.DisplayName;

            p.AutoIncrement = managementPackClassProperty.AutoIncrement;
            p.CaseSensitive = managementPackClassProperty.CaseSensitive;
            p.DefaultValue = managementPackClassProperty.DefaultValue;
            p.Key = managementPackClassProperty.Key;
            p.MaxLength = managementPackClassProperty.MaxLength;
            p.MaxValue = managementPackClassProperty.MaxValue;
            p.MinLength = managementPackClassProperty.MinLength;
            p.MinValue = managementPackClassProperty.MinValue;

            p.Required = managementPackClassProperty.Required;

            p.Type = managementPackClassProperty.Type.GetName();

            return p;
        }

        public static IEnumerable<ScsmClassProperty> ToScsmClassProperties(this IEnumerable<ManagementPackClassProperty> managementPackClassProperties)
        {
            return managementPackClassProperties.Select(p => p.ToScsmClassProperty());
        }
    }
}
