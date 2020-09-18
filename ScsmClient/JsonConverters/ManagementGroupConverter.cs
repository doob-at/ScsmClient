using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EnterpriseManagement;
using Newtonsoft.Json;
using Reflectensions.ExtensionMethods;

namespace BaseIT.SCSM.Client.JsonConverters
{
    public class ManagementGroupConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals<EnterpriseManagementGroup>() || objectType.InheritFromClass<EnterpriseManagementGroup>();
        }
    }
}
