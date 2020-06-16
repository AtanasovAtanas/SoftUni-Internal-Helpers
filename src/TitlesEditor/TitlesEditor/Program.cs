namespace TitlesEditor
{
    using System;
    using System.IO;
    using System.Linq;

    public class Program
    {
        public static void Main()
        {
            var path = @"C:\Users\User\Documents\SoftUni\interactive-content\EN";

            var directories = Directory.GetDirectories(path);

            foreach (var d in directories)
            {
                var files = Directory.GetFiles(d);

                foreach (var fp in files)
                {
                    var fileName = fp.Split('\\').Last();

                    if (fileName == "00-live.md" ||
                        fileName == "00-video.md" ||
                        fileName == "Lesson.md" ||
                        fileName.Contains("Homework-Results"))
                    {
                        continue;
                    }

                    var fileLines = File.ReadAllLines(fp);

                    var replacement = "[slide hideTitle]";
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        var currentLine = fileLines[i];

                        if (currentLine == "[slide]")
                        {
                            var nextLine = fileLines[i + 1];
                            if (nextLine.Contains("# Problem") || nextLine.Contains("# Solution:"))
                            {
                                fileLines[i] = replacement;
                            }
                        }

                    }

                    File.WriteAllText(
                        fp,
                        string.Join(
                            Environment.NewLine,
                            fileLines));
                }
            }
        }
    }
}
