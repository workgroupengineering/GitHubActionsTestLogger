using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using GitHubActionsTestLogger.Utils.Extensions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace GitHubActionsTestLogger;

internal static class TestSummary
{
    public static string Generate(IReadOnlyList<TestResult> testResults)
    {
        var buffer = new StringBuilder();

        buffer
            .AppendLine("# Test report")
            .AppendLine();

        // Summary
        {
            var passedCount = testResults.Count(r => r.Outcome == TestOutcome.Passed);
            var failedCount = testResults.Count(r => r.Outcome == TestOutcome.Failed);
            var skippedCount = testResults.Count(r => r.Outcome == TestOutcome.Skipped);
            var totalCount = testResults.Count;
            var totalDuration = testResults.Sum(r => r.Duration.TotalSeconds).Pipe(TimeSpan.FromSeconds);

            buffer
                .AppendLine("## Summary")
                .AppendLine()
                .Append("- 🟢 Passed: ")
                .Append("**").Append(passedCount.ToString("N0", CultureInfo.InvariantCulture)).AppendLine("**")
                .Append("- 🟡 Skipped: ")
                .Append("**").Append(skippedCount.ToString("N0", CultureInfo.InvariantCulture)).AppendLine("**")
                .Append("- 🔴 Failed: ")
                .Append("**").Append(failedCount.ToString("N0", CultureInfo.InvariantCulture)).AppendLine("**")
                .Append("- 🔵 Total: ")
                .Append("**").Append(totalCount.ToString("N0", CultureInfo.InvariantCulture)).AppendLine("**")
                .Append("- 🕑 Elapsed: ")
                .Append("**").Append(totalDuration.TotalSeconds.ToString("N3", CultureInfo.InvariantCulture)).AppendLine("s**")
                .AppendLine();
        }

        // Results
        {
            buffer
                .AppendLine("## Results")
                .AppendLine();

            foreach (var testResult in testResults)
            {
                buffer
                    .Append("- ### ")
                    .Append(testResult.Outcome switch
                    {
                        TestOutcome.Passed => "🟢",
                        TestOutcome.Failed => "🔴",
                        _ => "🟡",
                    })
                    .Append(' ')
                    .Append(testResult.TestCase.DisplayName)
                    .AppendLine()
                    .AppendLine();

                buffer
                    .Append("  - **Full name**: ")
                    .Append('`').Append(testResult.TestCase.FullyQualifiedName).AppendLine("`")
                    .Append("  - **Outcome**: ")
                    .AppendLine(testResult.Outcome.ToString())
                    .Append("  - **Duration**: ")
                    .Append(testResult.Duration.TotalSeconds.ToString("N3", CultureInfo.InvariantCulture)).AppendLine("s");

                if (!string.IsNullOrWhiteSpace(testResult.ErrorMessage))
                {
                    buffer
                        .AppendLine("  - **Error**:")
                        .AppendLine()
                        .AppendLine("```")
                        .AppendLine(testResult.ErrorMessage)
                        .AppendLine(testResult.ErrorStackTrace)
                        .AppendLine("```");
                }

                buffer.AppendLine();
            }
        }

        buffer.AppendLine();

        return buffer.ToString();
    }
}