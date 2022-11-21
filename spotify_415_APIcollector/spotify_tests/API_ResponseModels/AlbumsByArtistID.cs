using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotify_tests.API_ResponseModels.byid
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class AlbumsByArtistID
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("album_group", NullValueHandling = NullValueHandling.Ignore)]
        public AlbumGroup? AlbumGroup { get; set; }

        [JsonProperty("album_type")]
        public AlbumType AlbumType { get; set; }

        [JsonProperty("artists")]
        public Artist[] Artists { get; set; }

        [JsonProperty("available_markets")]
        public string[] AvailableMarkets { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("release_date_precision")]
        public ReleaseDatePrecision ReleaseDatePrecision { get; set; }

        [JsonProperty("total_tracks")]
        public long TotalTracks { get; set; }

        [JsonProperty("type")]
        public AlbumGroup Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public partial class Artist
    {
        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public partial class ExternalUrls
    {
        [JsonProperty("spotify")]
        public Uri Spotify { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public enum AlbumGroup { Album, AppearsOn, Single, Compilation };

    public enum AlbumType { Album, Compilation, Single };

    public enum TypeEnum { Artist };

    public enum ReleaseDatePrecision { Day, Month, Year };

    public partial class AlbumsByArtistID
    {
        public static AlbumsByArtistID FromJson(string json) => JsonConvert.DeserializeObject<AlbumsByArtistID>(json, spotify_tests.API_ResponseModels.byid.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this AlbumsByArtistID self) => JsonConvert.SerializeObject(self, spotify_tests.API_ResponseModels.byid.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                AlbumGroupConverter.Singleton,
                AlbumTypeConverter.Singleton,
                TypeEnumConverter.Singleton,
                ReleaseDatePrecisionConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class AlbumGroupConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AlbumGroup) || t == typeof(AlbumGroup?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "album":
                    return AlbumGroup.Album;
                case "appears_on":
                    return AlbumGroup.AppearsOn;
                case "single":
                    return AlbumGroup.Single;
                case "compilation":
                    return AlbumGroup.Compilation;
            }
            throw new Exception("Cannot unmarshal type AlbumGroup");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AlbumGroup)untypedValue;
            switch (value)
            {
                case AlbumGroup.Album:
                    serializer.Serialize(writer, "album");
                    return;
                case AlbumGroup.AppearsOn:
                    serializer.Serialize(writer, "appears_on");
                    return;
                case AlbumGroup.Single:
                    serializer.Serialize(writer, "single");
                    return;
            }
            throw new Exception("Cannot marshal type AlbumGroup");
        }

        public static readonly AlbumGroupConverter Singleton = new AlbumGroupConverter();
    }

    internal class AlbumTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AlbumType) || t == typeof(AlbumType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "album":
                    return AlbumType.Album;
                case "compilation":
                    return AlbumType.Compilation;
                case "single":
                    return AlbumType.Single;
            }
            throw new Exception("Cannot unmarshal type AlbumType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AlbumType)untypedValue;
            switch (value)
            {
                case AlbumType.Album:
                    serializer.Serialize(writer, "album");
                    return;
                case AlbumType.Compilation:
                    serializer.Serialize(writer, "compilation");
                    return;
                case AlbumType.Single:
                    serializer.Serialize(writer, "single");
                    return;
            }
            throw new Exception("Cannot marshal type AlbumType");
        }

        public static readonly AlbumTypeConverter Singleton = new AlbumTypeConverter();
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "artist")
            {
                return TypeEnum.Artist;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            if (value == TypeEnum.Artist)
            {
                serializer.Serialize(writer, "artist");
                return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }

    internal class ReleaseDatePrecisionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ReleaseDatePrecision) || t == typeof(ReleaseDatePrecision?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "day":
                    return ReleaseDatePrecision.Day;
                case "month":
                    return ReleaseDatePrecision.Month;
                case "year":
                    return ReleaseDatePrecision.Year;
            }
            throw new Exception("Cannot unmarshal type ReleaseDatePrecision");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ReleaseDatePrecision)untypedValue;
            switch (value)
            {
                case ReleaseDatePrecision.Day:
                    serializer.Serialize(writer, "day");
                    return;
                case ReleaseDatePrecision.Month:
                    serializer.Serialize(writer, "month");
                    return;
                case ReleaseDatePrecision.Year:
                    serializer.Serialize(writer, "year");
                    return;
            }
            throw new Exception("Cannot marshal type ReleaseDatePrecision");
        }

        public static readonly ReleaseDatePrecisionConverter Singleton = new ReleaseDatePrecisionConverter();
    }
}
