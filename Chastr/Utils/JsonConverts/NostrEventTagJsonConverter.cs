﻿using Chastr.Models.Nostr;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chastr.Utils.JsonConverts
{
    public class NostrEventTagJsonConverter : JsonConverter<NostrEventTag>
    {
        public override NostrEventTag? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var result = new NostrEventTag();
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Nostr Event Tags are an array");
            }

            reader.Read();
            var i = 0;
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                if (i == 0)
                {
                    result.TagIdentifier = reader.GetString();
                }
                else
                {
                    result.Data.Add(reader.GetString());
                }

                reader.Read();
                i++;
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, NostrEventTag value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartArray();
            writer.WriteStringValue(value.TagIdentifier);
            value.Data?.ForEach(writer.WriteStringValue);

            writer.WriteEndArray();
        }
    }
}
