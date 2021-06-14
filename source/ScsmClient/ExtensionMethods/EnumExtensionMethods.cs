using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using ScsmClient.Attributes;

namespace ScsmClient.ExtensionMethods
{
    public static class EnumExtensionMethods
    {

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute: Attribute
        {

            var enumType = enumValue.GetType();
            var name = Enum.GetName(enumType, enumValue);
            return enumType.GetField(name).GetCustomAttribute<TAttribute>();

        }

        public static bool HasId(this Enum enumValue)
        {
            return enumValue.GetAttribute<AsIdAttribute>() != null;
        }

        public static Guid Id(this Enum enumValue)
        {
            return enumValue.GetAttribute<AsIdAttribute>()?.Id ?? new Guid();
        }

        //public static Guid Id(this WellKnown.Incident.Impact enumValue)
        //{
        //    return enumValue.Id();
        //}

        //public static Guid Id(this WellKnown.Incident.Urgency enumValue)
        //{
        //    return enumValue.Id();
        //}
    }
}
