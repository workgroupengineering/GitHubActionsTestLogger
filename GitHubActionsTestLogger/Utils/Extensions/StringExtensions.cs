using System;
using System.Text;

namespace GitHubActionsTestLogger.Utils.Extensions;

internal static class StringExtensions
{
    extension(string str)
    {
        public string SubstringUntil(
            string sub,
            StringComparison comparison = StringComparison.Ordinal
        ) =>
            str.IndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[..index],
                _ => str,
            };

        public string SubstringUntilLast(
            string sub,
            StringComparison comparison = StringComparison.Ordinal
        ) =>
            str.LastIndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[..index],
                _ => str,
            };

        public string SubstringAfterLast(
            string sub,
            StringComparison comparison = StringComparison.Ordinal
        ) =>
            str.LastIndexOf(sub, comparison) switch
            {
                >= 0 and var index => str[(index + sub.Length)..],
                _ => "",
            };
    }

    extension(StringBuilder builder)
    {
        public StringBuilder Trim()
        {
            while (builder.Length > 0 && char.IsWhiteSpace(builder[0]))
                builder.Remove(0, 1);

            while (builder.Length > 0 && char.IsWhiteSpace(builder[^1]))
                builder.Remove(builder.Length - 1, 1);

            return builder;
        }
    }
}
