using Chastr.Models.Nostr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace Chastr.Websocket
{
    public static class RelaysPool
    {
        private static readonly List<string> _relays = new List<string>
        {
            "wss://freedom-relay.herokuapp.com/ws",
            "wss://relayer.fiatjaf.com/"
        };
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        public static void Startup()
        {
            foreach (var relay in _relays)
            {
                var url = new Uri(relay);
                ConnectToRelay(url);
            }
        }

        private static void ConnectToRelay(Uri url)
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

                    client.MessageReceived.Subscribe(msg =>
                    {
                        Debug.WriteLine(msg);
                    });

                    client.Start();

                    client.Send("[\"REQ\",\"3947502939534355\",{\"authors\":null},{\"author\":\"232f317db43d00bfbaf03f0246bfc8f980f85ae77af494727ec51136264606d5\"},{\"#p\":\"232f317db43d00bfbaf03f0246bfc8f980f85ae77af494727ec51136264606d5\"}]");

                    ExitEvent.WaitOne();
                }
            });

        }

        public static void AddRelay(string url)
        {
            _relays.Add(url);
            ConnectToRelay(new Uri(url));
        }
    }
}
