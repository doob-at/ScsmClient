using System;
using System.Linq;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Model
{
    public class ScsmClass : IScsmClass
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }

        public IScsmClassProperty[] Properties { get; set; }

        //public IScsmClass BaseClass { get; set; }

        public ScsmClass(ManagementPackClass managementPackClass)
        {
            Id = managementPackClass.Id;
            Name = managementPackClass.Name;
            DisplayName = managementPackClass.DisplayName;
            

            Properties = managementPackClass.PropertyCollection.Select(p => new ScsmClassProperty(p)).ToArray();
        }

        
    }
}
