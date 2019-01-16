using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoData
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Parcel
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("account_status")]
        public string AccountStatus { get; set; }

        [JsonProperty("bill_requestor_id")]
        public string BillRequestorId { get; set; }

        [JsonProperty("bill_year")]
        [JsonConverter(typeof(ParseStringConverterParcel))]
        public long BillYear { get; set; }

        [JsonProperty("billed_amount")]
        public string BilledAmount { get; set; }

        [JsonProperty("change_reason")]
        [JsonConverter(typeof(ParseStringConverterParcel))]
        public long ChangeReason { get; set; }

        [JsonProperty("draiange_benefit")]
        public string DraiangeBenefit { get; set; }

        [JsonProperty("drainage_acres")]
        public string DrainageAcres { get; set; }

        [JsonProperty("drainage_district")]
        public string DrainageDistrict { get; set; }

        [JsonProperty("forest_patrol_acres")]
        public string ForestPatrolAcres { get; set; }

        [JsonProperty("imps_value")]
        public string ImpsValue { get; set; }

        [JsonProperty("land_value")]
        public string LandValue { get; set; }

        [JsonProperty("levy_code")]
        [JsonConverter(typeof(ParseStringConverterParcel))]
        public long LevyCode { get; set; }

        [JsonProperty("omit_levy_code")]
        public string OmitLevyCode { get; set; }

        [JsonProperty("omit_year")]
        public string OmitYear { get; set; }

        [JsonProperty("paid_amount")]
        public string PaidAmount { get; set; }

        [JsonProperty("receivable_type")]
        public string ReceivableType { get; set; }

        [JsonProperty("tax_status")]
        public string TaxStatus { get; set; }
    }

    public partial class Parcel
    {
        public static Parcel FromJson(string json) => JsonConvert.DeserializeObject<Parcel>(json, GeoData.ConverterParcel.Settings);
    }

    public static class SerializeParcel
    {
        public static string ToJson(this Parcel self) => JsonConvert.SerializeObject(self, GeoData.ConverterParcel.Settings);
    }

    internal static class ConverterParcel
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

    internal class ParseStringConverterParcel : JsonConverter
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

        public static readonly ParseStringConverterParcel Singleton = new ParseStringConverterParcel();
    }
}