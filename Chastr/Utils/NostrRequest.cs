using Chastr.Models.Nostr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chastr.Utils
{
    public class NostrRequest
    {
        public IEnumerable<NostrSubscriptionFilter> Filters { get; private set; }

        public NostrRequest(IEnumerable<NostrSubscriptionFilter> filters)
        {
            Filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }
    }
}
