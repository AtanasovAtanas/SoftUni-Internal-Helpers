using System;

namespace TaskIdsFiller
{
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Program
    {
        static void Main()
        {
            //var taskIdPrefix = "java-fund";

            var regex = new Regex("taskId=\\\"(?<taskId>.*?)\\\"");

            var path = @"C:\Users\User\Documents\SoftUni\AtanasovAtanas\interactive-content\EN";

            var allFiles = Directory.GetFiles(
                path,
                "*.md",
                SearchOption.AllDirectories)
                .ToDictionary(k => k, v => FileInformation.Parse(v));

            var problemCounter = 1;
            var lesson = string.Empty;

            foreach (var kvp in allFiles)
            {
                var filePath = kvp.Key;
                var fileInfo = kvp.Value;

                if (fileInfo.Lesson != lesson)
                {
                    lesson = fileInfo.Lesson;
                    problemCounter = 1;
                }

                var fileContent = File.ReadAllText(filePath);

                var matches = regex.Matches(fileContent);

                foreach (Match match in matches)
                {
                    var oldId = match.Groups["taskId"].Value;
                    //var newId = $"{taskIdPrefix}-{fileInfo.Lesson}-problem-{problemCounter++}";
                    var newId = Guid.NewGuid().ToString();

                    fileContent = fileContent.Replace(oldId, newId);
                }

                File.WriteAllText(filePath, fileContent);
            }
        }
    }
}
