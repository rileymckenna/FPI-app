namespace GeoData
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Foreclosure
    {
        [JsonProperty("parcels")]
        public string Parcels { get; set; }
    }

    public partial class Foreclosure
    {
        public static List<Foreclosure> FromJson(string json) => JsonConvert.DeserializeObject<List<Foreclosure>>(json, GeoData.ConverterForeclosure.Settings);
    }

    public static class SerializeForeclosure
    {
        public static string ToJson(this List<Foreclosure> self) => JsonConvert.SerializeObject(self, GeoData.ConverterForeclosure.Settings);
    }

    internal static class ConverterForeclosure
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}