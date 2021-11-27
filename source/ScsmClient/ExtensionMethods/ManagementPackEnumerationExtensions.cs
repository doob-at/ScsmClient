using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;

namespace doob.ScsmClient.ExtensionMethods
{
    public static class ManagementPackEnumerationExtensions
    {

        public static string GetFullName(this ManagementPackEnumeration managementPackEnumeration)
        {

            if (managementPackEnumeration.Parent == null)
            {
                return managementPackEnumeration.DisplayName;
            }
            else
            {
                var parent = managementPackEnumeration.ManagementGroup.EntityTypes.GetEnumeration(managementPackEnumeration.Parent.Id);
                return $"{parent.GetFullName()}\\{managementPackEnumeration.DisplayName}";
            }
            
        }
    }
}
