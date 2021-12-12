using System;
using System.Collections.Generic;

namespace Chastr.Models.Nostr
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
