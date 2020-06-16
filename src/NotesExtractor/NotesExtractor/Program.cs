namespace NotesExtractor
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Program
    {
        public static void Main()
        {
            var path = "./";
            var pptxName = Console.ReadLine();

            File.Copy(
                $"./{pptxName}.pptx",
                $"./{pptxName}.zip");

            ZipFile.ExtractToDirectory(
                $"./{pptxName}.zip",
                $"{pptxName}");

            var notesSlidesPath = $"{path}{pptxName}/ppt/notesSlides";

            var notesSlides = Directory.GetFiles(
                    notesSlidesPath,
                    "*.xml")
                .OrderBy(s =>
                {
                    var number = int.Parse(string.Join(
                        string.Empty,
                        s.Where(char.IsDigit)));

                    return number;
                })
                .ToList();

            var fullScript = new StringBuilder();

            var regex = new Regex(@"<a:t>(?<content>.+?)</a:t>");
            foreach (var slide in notesSlides)
            {
                var xmlContent = File.ReadAllText(slide);
                
                var matches = regex.Matches(xmlContent)
                    .Select(m => m.Groups["content"].Value)
                    .ToList();

                var size = matches.Count;
                matches = matches.Take(size - 4).ToList();

                var slideScript = string.Join(
                    string.Empty,
                    matches);

                fullScript.AppendLine($"{slideScript}{Environment.NewLine}");
            }

            File.WriteAllText(
                "./full-script.txt",
                fullScript.ToString().TrimEnd());

            Directory.Delete(path + pptxName, true);
            File.Delete($"{path}{pptxName}.zip");
        }
    }
}
