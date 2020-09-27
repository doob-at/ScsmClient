using System;
using Microsoft.EnterpriseManagement;
using Newtonsoft.Json;
using Reflectensions.ExtensionMethods;

namespace ScsmClient.JsonConverters
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
