using PaleLotus.Cryptomus.Net.Models;

namespace PaleLotus.Cryptomus.Net;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal static class PaginationHelper
{
    public static async IAsyncEnumerable<TItem> Iterate<TRequest, TItem>(
        TRequest initialRequest,
        Func<TRequest, CancellationToken, Task<ApiResponse<IReadOnlyList<TItem>>>> fetch,
        Func<TRequest, int, TRequest> withPage,
        Func<TRequest, string?, TRequest> withCursor,
        Func<TRequest, int> getFirstPage,
        Func<TRequest, bool> hasExplicitCursor,
        Func<TRequest, string?> getCursor,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var firstPage = getFirstPage(initialRequest);
        var firstReq  = hasExplicitCursor(initialRequest)
            ? withCursor(initialRequest, getCursor(initialRequest))
            : withPage(initialRequest, firstPage);

        var res = await fetch(firstReq, ct).ConfigureAwait(false);
        foreach (var item in res.Result ?? []) yield return item;

        var p = res.Paginate;
        if (p is null) yield break;

        var useCursor = hasExplicitCursor(initialRequest) || !string.IsNullOrEmpty(p.NextCursor);
        if (useCursor)
        {
            var seen = new HashSet<string?>(StringComparer.Ordinal);
            var cursor = p.NextCursor;

            while (!string.IsNullOrEmpty(cursor))
            {
                ct.ThrowIfCancellationRequested();

                if (!seen.Add(cursor))
                    yield break;

                res = await fetch(withCursor(initialRequest, cursor), ct).ConfigureAwait(false);

                foreach (var item in res.Result ?? []) yield return item;
                cursor = res.Paginate?.NextCursor;
            }
            yield break;
        }

        if (p.HasPages != true) yield break;

        for (var page = firstPage + 1; ; page++)
        {
            ct.ThrowIfCancellationRequested();

            res = await fetch(withPage(initialRequest, page), ct).ConfigureAwait(false);

            var items = res.Result;
            if (items is null || items.Count == 0)
                yield break;

            foreach (var item in items) yield return item;

            if (res.Paginate?.HasPages != true)
                yield break;
        }
    }
}
