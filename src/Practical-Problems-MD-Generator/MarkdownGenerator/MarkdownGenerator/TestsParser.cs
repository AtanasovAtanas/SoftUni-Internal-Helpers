namespace MarkdownGenerator
{
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;

    public static class TestsParser
    {
        public static string GetTestsInMarkdown(string path)
        {
            ZipFile.ExtractToDirectory(path, "./tests");
            var sb = new StringBuilder();
            sb.AppendLine("[tests]");

            var tests = Directory.GetFiles(
                    "./tests",
                    "*.txt",
                    SearchOption.AllDirectories)
                .Where(f => !f.Contains("__MACOSX"))
                .OrderBy(f => f)
                .ToList();

            for (int i = 0; i < tests.Count; i += 2)
            {
                var inputTestFileName = tests[i];
                var inputContent = EscapeSpecialSymbols(File.ReadAllText(inputTestFileName)).Trim();

                if (inputTestFileName.Contains("000"))
                {
                    sb.AppendLine("[test open]");
                }
                else
                {
                    sb.AppendLine("[test]");
                }

                sb.AppendLine("[input]");
                sb.AppendLine(inputContent);
                sb.AppendLine("[/input]");

                var outputTestFileName = tests[i + 1];
                var outputContent = EscapeSpecialSymbols(File.ReadAllText(outputTestFileName)).Trim();

                sb.AppendLine("[output]");
                sb.AppendLine(outputContent);
                sb.AppendLine("[/output]");

                sb.AppendLine("[/test]");
            }

            sb.AppendLine("[/tests]");

            Directory.Delete("./tests", true);

            return sb.ToString().Trim();
        }

        private static string EscapeSpecialSymbols(string content)
        {
            return content
                .Replace("{", "\\{")
                .Replace("}", "\\}")
                .Replace("[", "\\[")
                .Replace("]", "\\]")
                .Replace("#", "\\#")
                .Replace("|", "\\|")
                .Replace("$", "\\$")
                .Replace(">", "\\>")
                .Replace("<", "\\<")
                .Replace("$", "\\$")
                .Replace(":", "\\:")
                .Replace("*", "\\*");
        }
    }
}