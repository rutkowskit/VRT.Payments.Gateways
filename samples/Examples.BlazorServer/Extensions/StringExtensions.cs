
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Examples.BlazorServer.Extensions;

internal static partial class StringExtensions
{
    private static readonly ConcurrentDictionary<string, string> CachedFormats = [];

    public static string WithReturnUrl(this string? url, BaseAppComponent callingComponent)
        => $"{url}?returnUrl={callingComponent.GetReturnUrl()}";

    public static string ToStringFormat(this string format)
    {
        if (string.IsNullOrWhiteSpace(format))
        {
            return string.Empty;
        }

        var result = CachedFormats.GetOrAdd(format, (input) =>
        {
            var regex = MatchFormatBlocks();
            var dic = new Dictionary<string, string>();
            int index = 0;
            string result = regex.Replace(input, match =>
            {
                if (dic.TryGetValue(match.Groups["block"].Value, out var replacer) is false)
                {
                    dic.Add(match.Groups["block"].Value, replacer = $"{{{index++}}}");
                }
                return replacer;
            });
            return result;
        });
        return result;
    }

    [GeneratedRegex(@"(?<block>\{[^{}]*\})")]
    private static partial Regex MatchFormatBlocks();
}
