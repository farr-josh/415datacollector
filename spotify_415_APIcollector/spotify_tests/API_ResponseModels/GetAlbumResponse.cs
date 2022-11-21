using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace spotify_tests.API_ResponseModels.Albums
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GetAlbumResponse
    {
        [JsonProperty("albums")]
        public Albums Albums { get; set; }
    }

    public partial class Albums
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
        public Uri Previous { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("album_type")]
        public AlbumTypeEnum AlbumType { get; set; }

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
        public AlbumTypeEnum Type { get; set; }

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
        public ArtistType Type { get; set; }

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

    public enum AlbumTypeEnum { Album, Compilation, Single };

    public enum ArtistType { Artist };

    public enum ReleaseDatePrecision { Day, Month, Year };

    public partial class GetAlbumResponse
    {
        public static GetAlbumResponse FromJson(string json) => JsonConvert.DeserializeObject<GetAlbumResponse>(json, spotify_tests.API_ResponseModels.Albums.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GetAlbumResponse self) => JsonConvert.SerializeObject(self, spotify_tests.API_ResponseModels.Albums.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                AlbumTypeEnumConverter.Singleton,
                ArtistTypeConverter.Singleton,
                ReleaseDatePrecisionConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class AlbumTypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AlbumTypeEnum) || t == typeof(AlbumTypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "album":
                    return AlbumTypeEnum.Album;
                case "compilation":
                    return AlbumTypeEnum.Compilation;
                case "single":
                    return AlbumTypeEnum.Single;
            }
            throw new Exception("Cannot unmarshal type AlbumTypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AlbumTypeEnum)untypedValue;
            switch (value)
            {
                case AlbumTypeEnum.Album:
                    serializer.Serialize(writer, "album");
                    return;
                case AlbumTypeEnum.Compilation:
                    serializer.Serialize(writer, "compilation");
                    return;
                case AlbumTypeEnum.Single:
                    serializer.Serialize(writer, "single");
                    return;
            }
            throw new Exception("Cannot marshal type AlbumTypeEnum");
        }

        public static readonly AlbumTypeEnumConverter Singleton = new AlbumTypeEnumConverter();
    }

    internal class ArtistTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ArtistType) || t == typeof(ArtistType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "artist")
            {
                return ArtistType.Artist;
            }
            throw new Exception("Cannot unmarshal type ArtistType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ArtistType)untypedValue;
            if (value == ArtistType.Artist)
            {
                serializer.Serialize(writer, "artist");
                return;
            }
            throw new Exception("Cannot marshal type ArtistType");
        }

        public static readonly ArtistTypeConverter Singleton = new ArtistTypeConverter();
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