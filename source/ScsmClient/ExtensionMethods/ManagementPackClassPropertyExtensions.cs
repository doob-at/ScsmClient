using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.Model;

namespace ScsmClient.ExtensionMethods
{
    public static class ManagementPackClassPropertyExtensions
    {
        public static ScsmClassProperty ToScsmClassProperty(this ManagementPackClassProperty managementPackClassProperty)
        {
            return new ScsmClassProperty(managementPackClassProperty);
        }

        public static IEnumerable<ScsmClassProperty> ToScsmClassProperties(this IEnumerable<ManagementPackClassProperty> managementPackClassProperties)
        {
            return managementPackClassProperties.Select(p => p.ToScsmClassProperty());
        }
    }
}
