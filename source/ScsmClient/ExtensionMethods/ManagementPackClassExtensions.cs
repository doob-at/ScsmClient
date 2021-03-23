using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.Model;

namespace ScsmClient.ExtensionMethods
{
    public static class ManagementPackClassExtensions
    {
        public static ScsmClass ToScsmClass(this ManagementPackClass managementPackClass)
        {
            return new ScsmClass(managementPackClass);
        }

        public static IEnumerable<ScsmClass> ToScsmClasses(this IEnumerable<ManagementPackClass> managementPackClasses)
        {
            return managementPackClasses.Select(c => c.ToScsmClass());
        }
    }
}
