using Chastr.Models.Nostr;
using Chastr.Services;
using Chastr.Utils;
using Chastr.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;
using Xamarin.Essentials;

namespace Chastr.Websocket
{
    public static class RelaysPool
    {
        private static readonly List<string> _relays = new List<string>
        {
            "wss://freedom-relay.herokuapp.com/ws",
            //"wss://relayer.fiatjaf.com/"
        };
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        public static async void Startup()
        {
            var dataStore = new DataStore<Models.Contact>();
            var contacts = await dataStore.GetItemsAsync(true);
            var ownPubKey = (await SecureStorage.GetAsync(Constants.PUBLIC_KEY)).ToString().ToLower();
            var filters = new List<NostrSubscriptionFilter>
            {
                new NostrSubscriptionFilter
                {
                    Authors = contacts.Select(c => c.PubKey).ToArray(),
                    PublicKey = ownPubKey,
                    Kind = 4
                },
                new NostrSubscriptionFilter
                {
                    Author = ownPubKey,
                    Kind = 4
                }
            };
            var request = new NostrRequest(filters);

            foreach (var relay in _relays)
            {
                var url = new Uri(relay);
                ConnectToRelay(url, request.ToRequestJson());
            }
        }

        private static void ConnectToRelay(Uri url, string requestPubKeys)
        {
            Task.Run(() =>
            {
                using (var client = new WebsocketClient(url))
                {

                    client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                    client.ErrorReconnectTimeout = TimeSpan.FromSeconds(30);
                    client.ReconnectionHappened.Subscribe(info =>
                    {
                        Debug.WriteLine(info);

                    });
                    client.DisconnectionHappened.Subscribe(info =>
                    {
                        Debug.WriteLine(info);
                    });

                    client.MessageReceived.Subscribe(async msg =>
                    {
                        Debug.WriteLine(msg);
                        await EventHandler.Handle(msg.Text);
                    });

                    client.Start();

                    client.Send(requestPubKeys);

                    ExitEvent.WaitOne();
                }
            });

        }

        public static void AddRelay(string url)
        {
            // TODO
            _relays.Add(url);
            ConnectToRelay(new Uri(url), "");
        }
    }
}
