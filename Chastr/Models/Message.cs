using Chastr.Utils;
using Chastr.Utils.JsonConverts;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Chastr.Models
{
    public class Message : IItem
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string PublicKey { get; set; }

        [JsonConverter(typeof(UnixTimestampSecondsJsonConverter))]
        public DateTimeOffset? CreatedAt { get; set; }

        public NostrKind Kind { get; set; }

        public string Content { get; set; }

        [OneToMany("MessageId")]
        public List<MessageTag> Tags { get; set; }

        public string Signature { get; set; }
    }

    public class MessageTag : IItem
    {
        [PrimaryKey]
        public string Id { get; set; }

        [ForeignKey(typeof(Message))]
        public string MessageId { get; set; }

        public string TagIdentifier { get; set; }
        public List<string> Data { get; set; } = new List<string>();
    }
}
