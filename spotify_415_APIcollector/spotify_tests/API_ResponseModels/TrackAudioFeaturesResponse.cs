using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace spotify_tests.API_ResponseModels.audio
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class TrackAudioFeatures
    {
        [JsonProperty("audio_features")]
        public AudioFeature[] AudioFeatures { get; set; }
    }

    public partial class AudioFeature
    {
        [JsonProperty("acousticness")]
        public double Acousticness { get; set; }

        [JsonProperty("analysis_url")]
        public Uri AnalysisUrl { get; set; }

        [JsonProperty("danceability")]
        public double Danceability { get; set; }

        [JsonProperty("duration_ms")]
        public long DurationMs { get; set; }

        [JsonProperty("energy")]
        public double Energy { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("instrumentalness")]
        public double Instrumentalness { get; set; }

        [JsonProperty("key")]
        public long Key { get; set; }

        [JsonProperty("liveness")]
        public double Liveness { get; set; }

        [JsonProperty("loudness")]
        public double Loudness { get; set; }

        [JsonProperty("mode")]
        public long Mode { get; set; }

        [JsonProperty("speechiness")]
        public double Speechiness { get; set; }

        [JsonProperty("tempo")]
        public double Tempo { get; set; }

        [JsonProperty("time_signature")]
        public long TimeSignature { get; set; }

        [JsonProperty("track_href")]
        public Uri TrackHref { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("valence")]
        public double Valence { get; set; }
    }

    public partial class TrackAudioFeatures
    {
        public static TrackAudioFeatures FromJson(string json) => JsonConvert.DeserializeObject<TrackAudioFeatures>(json, spotify_tests.API_ResponseModels.audio.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this TrackAudioFeatures self) => JsonConvert.SerializeObject(self, spotify_tests.API_ResponseModels.audio.Converter.Settings);
    }

    internal static class Converter
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