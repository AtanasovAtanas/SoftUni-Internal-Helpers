namespace TaskIdExtractor
{
    using System;
    using System.Linq;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;

    public class Program
    {
        public static void Main()
        {
            var counts = new Dictionary<string, int>();

            var path = @"C:\Users\User\Documents\SoftUni\AtanasovAtanas\interactive-content\EN";
            var regex = new Regex("taskId=\\\"(?<taskId>.*?)\\\"");

            //var taskOrder = 1;

            var allFiles = Directory.GetFiles(path, "*.md", SearchOption.AllDirectories);
            foreach (var fp in allFiles)
            {
                var fileName = fp.Split('\\').Last();

                var fileContent = File.ReadAllText(fp);

                //var idMatches = regex.Matches(fileContent);

                //foreach (Match m in idMatches)
                //{
                //    var newId = $"java-fundamentals-data-types-lesson-{taskOrder++}";

                //    fileContent = new Regex(Regex.Escape(m.Groups["taskId"].Value))
                //        .Replace(
                //            fileContent,
                //            newId,
                //            1);
                //}

                //File.WriteAllText(fp, fileContent);

                var matches = regex.Matches(fileContent);
                foreach (Match match in matches)
                {
                    var key = match.Groups["taskId"].Value;

                    if (!counts.ContainsKey(key))
                    {
                        counts[key] = 0;
                    }

                    counts[key] += 1;
                    Console.WriteLine($"{fileName} -> {key}");
                }
            }

            //foreach (var kvp in counts.OrderByDescending(c => c.Value))
            //{
            //    Console.WriteLine($"{kvp.Key} -> {kvp.Value}");
            //}
        }
    }
}
