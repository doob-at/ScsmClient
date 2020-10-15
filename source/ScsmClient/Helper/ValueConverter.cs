using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement.Configuration;
using Reflectensions.ExtensionMethods;
using ScsmClient.ExtensionMethods;

namespace ScsmClient.Helper
{
    public class ValueConverter
    {
        private readonly SCSMClient _scsmClient;

        public ValueConverter(SCSMClient scsmClient)
        {
            _scsmClient = scsmClient;
        }


        public object NormalizeValue(object value, ManagementPackProperty property)
        {
            switch (property.Type)
            {
                case ManagementPackEntityPropertyTypes.@enum:
                    return NormalizeEnum(value, property.EnumType.GetElement());
                case ManagementPackEntityPropertyTypes.datetime:
                {
                    return NormalizeDate(value);
                }
                default:
                    return value;
            }
        }

        private object NormalizeEnum(object enumValue, ManagementPackEnumeration managementPackEnumeration)
        {
            if (enumValue is Guid guid)
            {
                return _scsmClient.Enumeration().GetEnumerationChildById(managementPackEnumeration, guid);
            }
            else if (enumValue is string str)
            {
                if (str.IsGuid())
                {
                    return NormalizeEnum(str.ToGuid(), managementPackEnumeration);
                }
                return _scsmClient.Enumeration().GetEnumerationChildByName(managementPackEnumeration, str);
            }
            else if (enumValue is Enum enu)
            {
                if (enu.HasId())
                {
                    return NormalizeEnum(enu.Id(), managementPackEnumeration);
                }

            }

            return enumValue;
        }

        public string NormalizeValueForCriteria(object value, ManagementPackProperty property)
        {

            switch (property.Type)
            {
                case ManagementPackEntityPropertyTypes.@enum:
                    return NormalizeEnumForCriteria(value, property.EnumType.GetElement());
                default:
                    return value.ToString();
            }
        }
        private string NormalizeEnumForCriteria(object enumValue, ManagementPackEnumeration managementPackEnumeration)
        {
            if (enumValue is Guid guid)
            {
                return guid.ToString();
            }
            else if (enumValue is string str)
            {
                if (str.IsGuid())
                {
                    return NormalizeEnumForCriteria(str.ToGuid(), managementPackEnumeration);
                }
                return _scsmClient.Enumeration().GetEnumerationChildByName(managementPackEnumeration, str)?.Id.ToString();
            }

            return enumValue.ToString();
        }

        private string NormalizeDate(object value)
        {
            if (!(value is DateTime dt))
            {
                dt = ToNullableDateTime(value.ToString());
            }
            return dt.ToString("yyyy-MM-ddTHH:mm:ss.FFFFF");
        }


        public string NormalizeGenericValueForCriteria(string value, string propertyname)
        {
            if (propertyname.StartsWith("G:", StringComparison.OrdinalIgnoreCase))
            {
                propertyname = propertyname.Substring(2);
            }

            switch (propertyname.ToLower())
            {
                case "lastmodified":
                case "timeadded":
                {
                    return NormalizeDate(value);
                }
            }

            return value;
        }

        public DateTime ToNullableDateTime(string value)
        {

            DateTime dateTime = default(DateTime);

            List<string> formats = new List<string>();

            
                formats = new List<string>{

                    "d.M.yyyy",
                    "dd.M.yyyy",
                    "d.MM.yyyy",
                    "dd.MM.yyyy",

                    "d.M.yyyy HH:mm",
                    "dd.M.yyyy HH:mm",
                    "d.MM.yyyy HH:mm",
                    "dd.MM.yyyy HH:mm",

                    "d.M.yyyy H:mm",
                    "dd.M.yyyy H:mm",
                    "d.MM.yyyy H:mm",
                    "dd.MM.yyyy H:mm",

                    "d.M.yyyy HH:mm:ss.FFFFF",
                    "d.MM.yyyy HH:mm:ss.FFFFF",
                    "dd.M.yyyy HH:mm:ss.FFFFF",
                    "dd.MM.yyyy HH:mm:ss.FFFFF",

                    "d.M.yyyy H:mm:ss.FFFFF",
                    "d.MM.yyyy H:mm:ss.FFFFF",
                    "dd.M.yyyy H:mm:ss.FFFFF",
                    "dd.MM.yyyy H:mm:ss.FFFFF",

                    "M/d/yyyy h:mm:ss tt",
                    "M/d/yyyy h:mm:ss.FFFFF tt",
                    "M/d/yyyy h:mm tt",
                    "MM/dd/yyyy hh:mm:ss.FFFFF",
                    "M/d/yyyy h:mm:ss.FFFFF",
                    "M/d/yyyy hh:mm tt",
                    "M/d/yyyy hh tt",
                    "M/d/yyyy h:mm",
                    "M/d/yyyy h:mm",
                    "MM/dd/yyyy hh:mm",
                    "M/dd/yyyy hh:mm"
                };
            
            

            if (DateTime.TryParseExact(value, formats.ToArray(),
                CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.AssumeLocal, out dateTime))
            {
                return dateTime;
            }

            throw new InvalidCastException($"Can't cast '{value}' to DateTime!");
        }
    }
}
