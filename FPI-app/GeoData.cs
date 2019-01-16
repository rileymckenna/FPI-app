using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//    using GeoData;
//
//    var welcome = Welcome.FromJson(jsonString);


namespace GeoData
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GeoParcel
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("features")]
        public List<Feature> Features { get; set; }
    }

    public partial class Feature
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
    }

    public partial class Geometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public List<List<List<List<double>>>> Coordinates { get; set; }
    }

    public partial class Properties
    {
        [JsonProperty("shape_area")]
        public string ShapeArea { get; set; }

        [JsonProperty("objectid")]
        [JsonConverter(typeof(ParseStringConverterGeoParcel))]
        public long Objectid { get; set; }

        [JsonProperty("shape_leng")]
        public string ShapeLeng { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("major")]
        public string Major { get; set; }

        [JsonProperty("minor")]
        public string Minor { get; set; }
    }

    public partial class GeoParcel
    {
        public static GeoParcel FromJson(string json) => JsonConvert.DeserializeObject<GeoParcel>(json, GeoData.ConverterGeoParcel.Settings);
    }

    public static class SerializeGeoParcel
    {
        public static string ToJson(this GeoParcel self) => JsonConvert.SerializeObject(self, GeoData.ConverterGeoParcel.Settings);
    }

    internal static class ConverterGeoParcel
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

    internal class ParseStringConverterGeoParcel : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverterGeoParcel Singleton = new ParseStringConverterGeoParcel();
    }
}
