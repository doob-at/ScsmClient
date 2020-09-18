using System;
using BaseIT.SCSM.Client.JsonConverters;
using Newtonsoft.Json.Converters;
using Reflectensions.JsonConverters;

namespace BaseIT.SCSM.Client
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
