using Chastr.Models.Nostr;
using Chastr.Services;
using Chastr.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chastr.Websocket
{
    public static class EventHandler
    {
        private const string PREFIX = "EVENT";

        public static async Task Handle(string msg)
        {
            if (!msg.StartsWith($"[\"{PREFIX}"))
            {
                return;
            }

            var json = JsonDocument.Parse(msg).RootElement;
            var e = JsonSerializer.Deserialize<NostrEvent>(json[2].GetRawText());
            if (e.Verify())
            {
                var dataStore = new DataStore<Models.Message>();
                var contacts = await dataStore.AddItemAsync(e.ToMessage());
            }
        }
    }
}
