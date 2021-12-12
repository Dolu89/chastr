using Chastr.Utils.JsonConverts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chastr.Models.Nostr
{
    [JsonConverter(typeof(NostrEventTagJsonConverter))]
    public class NostrEventTag
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string TagIdentifier { get; set; }
        public List<string> Data { get; set; } = new List<string>();

        public NostrEvent Event { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(Data.Prepend(TagIdentifier));
        }
    }
}
