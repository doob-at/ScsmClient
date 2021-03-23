using System;
using ScsmClient.Model;

namespace ScsmClient.SharedModels.Models
{
    public class ScsmClass
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }

        public ScsmClassProperty[] Properties { get; set; }

        public string BaseClassName { get; set; }
 
    }
}
