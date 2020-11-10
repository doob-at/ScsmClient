using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Configuration;

namespace ScsmClient.CriteriaParser
{
    public class EvaluatorPropertyInfo
    {
        public string Path { get; }
        public ManagementPackProperty Property { get; }
        public GenericProperty GenericProperty { get; }
        public PropertyInfoType Type { get; }

        public EvaluatorPropertyInfo(string path, ManagementPackProperty property)
        {
            Path = path;
            Property = property;
            Type = PropertyInfoType.Property;
        }

        public EvaluatorPropertyInfo(string path, GenericProperty property)
        {
            Path = path;
            GenericProperty = property;
            Type = PropertyInfoType.GenericProperty;
        }

        public EvaluatorPropertyInfo(string path)
        {
            Path = path;
            Type = PropertyInfoType.Value;
        }
    }

    public enum PropertyInfoType
    {
        Value,
        Property,
        GenericProperty
    }
}
