using System;
using Microsoft.EnterpriseManagement.Configuration;
using Reflectensions.ExtensionMethods;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Model
{
    public class ScsmClassProperty : IScsmClassProperty
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public bool AutoIncrement { get; set; }
        public bool CaseSensitive { get; set; }
        public bool Key { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }

        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }

        public string Type { get; set; }

        public ScsmClassProperty(ManagementPackClassProperty managementPackClassProperty)
        {
            Id = managementPackClassProperty.Id;
            Name = managementPackClassProperty.Name;
            DisplayName = managementPackClassProperty.DisplayName;

            AutoIncrement = managementPackClassProperty.AutoIncrement;
            CaseSensitive = managementPackClassProperty.CaseSensitive;
            DefaultValue = managementPackClassProperty.DefaultValue;
            Key = managementPackClassProperty.Key;
            MaxLength = managementPackClassProperty.MaxLength;
            MaxValue = managementPackClassProperty.MaxValue;
            MinLength = managementPackClassProperty.MinLength;
            MinValue = managementPackClassProperty.MinValue;

            Required = managementPackClassProperty.Required;

            Type = managementPackClassProperty.Type.GetName();
        }
    }
}