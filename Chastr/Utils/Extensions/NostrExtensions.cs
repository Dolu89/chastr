using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using Chastr.Models.Nostr;
using NBitcoin.Secp256k1;

namespace Chastr.Utils.Extensions
{
    public static class NostrExtensions
    {
        private const string REQUEST = "REQ";

        public static string ToJson(this NostrEvent nostrEvent)
        {
            return
                $"[0,\"{nostrEvent.PublicKey}\",{nostrEvent.CreatedAt?.ToUnixTimeSeconds()},{nostrEvent.Kind},[{string.Join(',', nostrEvent.Tags.Select(tag => tag.ToString()))}],\"{nostrEvent.Content}\"]";
        }

        public static string ComputeId(this NostrEvent nostrEvent)
        {
            return nostrEvent.ToJson().ComputeSha256Hash().ToHex();
        }

        public static string ComputeSignature(this NostrEvent nostrEvent, ECPrivKey priv)
        {
            return nostrEvent.ToJson().ComputeSignature(priv);
        }

        public static bool Verify(this NostrEvent nostrEvent)
        {
            var hash = nostrEvent.ToJson().ComputeSha256Hash();
            if (hash.ToHex() != nostrEvent.Id)
            {
                return false;
            }
            var pub = nostrEvent.GetPublicKey();
            if (!SecpSchnorrSignature.TryCreate(nostrEvent.Signature.DecodHexData(), out var sig))
            {
                return false;
            }

            return pub.SigVerifyBIP340(sig, hash);
        }

        public static ECXOnlyPubKey GetPublicKey(this NostrEvent nostrEvent)
        {
            return Context.Instance.CreateXOnlyPubKey(StringToByteArray(nostrEvent.PublicKey));
        }

        // https://stackoverflow.com/a/311179
        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static IQueryable<NostrEvent> Filter(this IQueryable<NostrEvent> events, params NostrSubscriptionFilter[] filters)
        {
            IQueryable<NostrEvent> result = null;
            foreach (var filter in filters)
            {
                var filterQuery = events;
                if (!string.IsNullOrEmpty(filter.Id))
                {
                    filterQuery = filterQuery.Where(e => e.Id == filter.Id);
                }

                if (filter.Kind != null)
                {
                    filterQuery = filterQuery.Where(e => e.Kind == filter.Kind);
                }

                if (filter.Since != null)
                {
                    filterQuery = filterQuery.Where(e => e.CreatedAt > filter.Since);
                }

                if (!string.IsNullOrEmpty(filter.Author))
                {
                    filterQuery = filterQuery.Where(e => e.PublicKey == filter.Author);
                }

                var authors = filter.Authors?.Where(s => !string.IsNullOrEmpty(s))?.ToArray();
                if (authors?.Any() is true)
                {
                    filterQuery = filterQuery.Where(e => authors.Contains(e.PublicKey));
                }

                if (!string.IsNullOrEmpty(filter.EventId))
                {
                    filterQuery = filterQuery.Where(e =>
                        e.Tags.Any(tag => tag.TagIdentifier == "e" && tag.Data[1] == filter.EventId));
                }

                if (!string.IsNullOrEmpty(filter.PublicKey))
                {
                    filterQuery = filterQuery.Where(e =>
                        e.Tags.Any(tag => tag.TagIdentifier == "p" && tag.Data[1] == filter.PublicKey));
                }

                result = result is null ? filterQuery : result.Union(filterQuery);

            }

            return result;
        }

        public static IEnumerable<NostrEvent> Filter(this IEnumerable<NostrEvent> events, params NostrSubscriptionFilter[] filters)
        {
            IEnumerable<NostrEvent> result = null;
            foreach (var filter in filters)
            {
                var filterQuery = events;
                if (!string.IsNullOrEmpty(filter.Id))
                {
                    filterQuery = filterQuery.Where(e => e.Id == filter.Id);
                }

                if (filter.Kind != null)
                {
                    filterQuery = filterQuery.Where(e => e.Kind == filter.Kind);
                }

                if (filter.Since != null)
                {
                    filterQuery = filterQuery.Where(e => e.CreatedAt > filter.Since);
                }

                if (!string.IsNullOrEmpty(filter.Author))
                {
                    filterQuery = filterQuery.Where(e => e.PublicKey == filter.Author);
                }

                var authors = filter.Authors?.Where(s => !string.IsNullOrEmpty(s))?.ToArray();
                if (authors?.Any() is true)
                {
                    filterQuery = filterQuery.Where(e => authors.Contains(e.PublicKey));
                }

                if (!string.IsNullOrEmpty(filter.EventId))
                {
                    filterQuery = filterQuery.Where(e =>
                        e.Tags.Any(tag => tag.TagIdentifier == "e" && tag.Data[1] == filter.EventId));
                }

                if (!string.IsNullOrEmpty(filter.PublicKey))
                {
                    filterQuery = filterQuery.Where(e =>
                        e.Tags.Any(tag => tag.TagIdentifier == "p" && tag.Data[1] == filter.PublicKey));
                }

                result = result is null ? filterQuery : result.Union(filterQuery);

            }

            return result;
        }

        public static string ToRequestJson(this NostrRequest request)
        {
            var id = Guid.NewGuid().ToString();
            var reqJson = new List<object> { REQUEST, id };
            reqJson.AddRange(request.Filters);
            return JsonSerializer.Serialize(reqJson);
        }

        public static Models.Message ToMessage(this NostrEvent e)
        {
            return new Models.Message
            {
                Id = e.Id,
                Content = e.Content,
                CreatedAt = e.CreatedAt,
                Kind = (NostrKind)e.Kind,
                PublicKey = e.PublicKey,
                Signature = e.Signature,
                Tags = e.Tags.Select(t => new Models.MessageTag { Id = t.Id, TagIdentifier = t.TagIdentifier, Data = t.Data, }).ToList()
            };
        }
    }
}
