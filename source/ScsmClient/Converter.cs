using System;
using Newtonsoft.Json.Converters;
using Reflectensions.JsonConverters;
using ScsmClient.JsonConverters;

namespace ScsmClient
{
    internal static class Converter
    {

        private static readonly Lazy<Reflectensions.Json> lazyJson = new Lazy<Reflectensions.Json>(() => new Reflectensions.Json()
            .RegisterJsonConverter<StringEnumConverter>()
            .RegisterJsonConverter<DefaultDictionaryConverter>()
            .RegisterJsonConverter<ManagementGroupConverter>()
        );

        public static Reflectensions.Json Json => lazyJson.Value;


    }

}
