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
        public static ScsmClassProperty ToScsmClassProperty(this ManagementPackProperty managementPackProperty)
        {
            var p = new ScsmClassProperty();
            p.Id = managementPackProperty.Id;
            p.Name = managementPackProperty.Name;
            p.DisplayName = managementPackProperty.DisplayName;

            p.AutoIncrement = managementPackProperty.AutoIncrement;
            p.CaseSensitive = managementPackProperty.CaseSensitive;
            p.DefaultValue = managementPackProperty.DefaultValue;
            p.Key = managementPackProperty.Key;
            p.MaxLength = managementPackProperty.MaxLength;
            p.MaxValue = managementPackProperty.MaxValue;
            p.MinLength = managementPackProperty.MinLength;
            p.MinValue = managementPackProperty.MinValue;

            p.Required = managementPackProperty.Required;

            p.Type = managementPackProperty.Type.GetName();

            return p;
        }

        public static IEnumerable<ScsmClassProperty> ToScsmClassProperties(this IEnumerable<ManagementPackProperty> managementPackProperties)
        {
            return managementPackProperties.Select(p => p.ToScsmClassProperty());
        }
    }
}
