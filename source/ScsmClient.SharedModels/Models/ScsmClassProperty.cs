using System;
using ScsmClient.SharedModels.Models;

namespace ScsmClient.Model
{
    public class ScsmClassProperty
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

    }
}