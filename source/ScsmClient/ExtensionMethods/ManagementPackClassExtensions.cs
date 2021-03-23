using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.Model;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.ExtensionMethods
{
    public static class ManagementPackClassExtensions
    {
        public static ScsmClass ToScsmClass(this ManagementPackClass managementPackClass)
        {
            var c = new ScsmClass();
            c.Id = managementPackClass.Id;
            c.Name = managementPackClass.Name;
            c.DisplayName = managementPackClass.DisplayName;
            c.BaseClassName = managementPackClass.Base?.GetElement()?.Name;

            c.Properties = managementPackClass.PropertyCollection.ToScsmClassProperties().ToArray();

            return c;
        }

        public static IEnumerable<ScsmClass> ToScsmClasses(this IEnumerable<ManagementPackClass> managementPackClasses)
        {
            return managementPackClasses.Select(c => c.ToScsmClass());
        }
    }
}
