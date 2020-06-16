namespace SubtitlesMerger
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Program
    {
        public static void Main()
        {
            var path = @"./";
            var subtitlePaths = GetSubtitlePaths(path, "sbv");

            var dictionary = ParseSubtitles(subtitlePaths);

            var durations = GetDurations("./durations.txt");

            foreach (var kvp in dictionary.Skip(1))
            {
                var ts = durations.Dequeue();

                foreach (var value in kvp.Value)
                {
                    value.StartTime += ts;
                    value.EndTime += ts;
                }
            }

            var result = GetResult(dictionary);

            File.WriteAllText(
                "./result.sbv",
                result);
        }

        private static Dictionary<string, List<Subtitle>> ParseSubtitles(IList<string> filesPath)
        {
            var dictionary = new Dictionary<string, List<Subtitle>>();

            var regex = new Regex(@"(?<startTime>\d+:\d+:\d+\.\d+)\,(?<endTime>\d+:\d+:\d+\.\d+)");

            foreach (var fp in filesPath)
            {
                var lines = File.ReadAllLines(fp)
                    .Where(l => l != string.Empty)
                    .ToList();

                var sb = new StringBuilder();
                var subtitles = new List<Subtitle>();
                var currentSubtitle = new Subtitle();

                foreach (var line in lines)
                {
                    var match = regex.Match(line);

                    if (match.Success)
                    {
                        currentSubtitle.Content = sb.ToString().TrimEnd();
                        subtitles.Add(new Subtitle
                        {
                            StartTime = currentSubtitle.StartTime,
                            EndTime = currentSubtitle.EndTime,
                            Content = currentSubtitle.Content
                        });

                        sb.Clear();

                        var startTime = TimeSpan.Parse(match.Groups["startTime"].Value);
                        var endTime = TimeSpan.Parse(match.Groups["endTime"].Value);

                        currentSubtitle.StartTime = startTime;
                        currentSubtitle.EndTime = endTime;
                        currentSubtitle.Content = string.Empty;
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }

                currentSubtitle.Content = sb.ToString().TrimEnd();
                subtitles.Add(new Subtitle
                {
                    StartTime = currentSubtitle.StartTime,
                    EndTime = currentSubtitle.EndTime,
                    Content = currentSubtitle.Content
                });

                subtitles.RemoveAt(0);
                dictionary[fp] = subtitles;
            }

            return dictionary;
        }

        private static IList<string> GetSubtitlePaths(string path, string ext)
        {
            return Directory.GetFiles(
                    path,
                    $"*.{ext}",
                    SearchOption.TopDirectoryOnly)
                .OrderBy(fp => fp)
                .ToList();
        }

        private static string GetResult(Dictionary<string, List<Subtitle>> dictionary)
        {
            var sb = new StringBuilder();

            foreach (var dictionaryValue in dictionary.Values)
            {
                foreach (var subtitle in dictionaryValue)
                {
                    sb.AppendLine(subtitle.ToString());
                }
            }

            //dictionary
            //    .SelectMany(kvp => kvp.Value)
            //    .Select(s => s.ToString())
            //    .ToList()
            //    .ForEach(s => sb.AppendLine(s));

            return sb.ToString().TrimEnd();
        }

        private static Queue<TimeSpan> GetDurations(string durationsPath)
        {
            var durations = File.ReadAllLines(durationsPath)
                .Select(TimeSpan.Parse)
                .ToList();

            for (int i = 1; i < durations.Count; i++)
            {
                durations[i] += durations[i - 1];
            }

            return new Queue<TimeSpan>(durations);
        }
    }
}
