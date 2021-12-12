using Chastr.Utils.JsonConverts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Chastr.Models.Nostr
{
    public class NostrSubscriptionFilter
    {
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Id { get; set; }

        [JsonPropertyName("author")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Author { get; set; }

        [JsonPropertyName("authors")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[] Authors { get; set; }

        [JsonPropertyName("kind")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Kind { get; set; }

        [JsonPropertyName("#e")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string EventId { get; set; }

        [JsonPropertyName("#p")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PublicKey { get; set; }

        [JsonPropertyName("since")]
        [JsonConverter(typeof(UnixTimestampSecondsJsonConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTimeOffset? Since { get; set; }
    }
}
