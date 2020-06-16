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
                $"{path}{pptxName}.pptx",
                $"{path}{pptxName}.zip");

            ZipFile.ExtractToDirectory(
                $"{path}{pptxName}.zip",
                $"{pptxName}");

            var notesSlidesPath = $"{path}{pptxName}/ppt/notesSlides";

            var notesSlides = Directory.GetFiles(
                    notesSlidesPath,
                    "*.xml")
                .OrderBy(s => int.Parse(string.Join(
                    string.Empty,
                    s.Where(char.IsDigit))))
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
                $"{path}full-script.txt",
                fullScript.ToString().TrimEnd());

            Directory.Delete(path + pptxName, true);
            File.Delete($"{path}{pptxName}.zip");
        }
    }
}
