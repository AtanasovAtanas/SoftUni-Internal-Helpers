namespace TaskIdsFiller
{
    public class FileInformation
    {
        public static FileInformation Parse(string path)
        {
            var parts = path.Split('\\');
            var fileName = parts[parts.Length - 1];
            var lesson = parts[parts.Length - 2];

            return new FileInformation
            {
                Filename =  fileName,
                Lesson =  lesson,
                Path = path
            };
        }

        public string Filename { get; set; }

        public string Lesson { get; set; }

        public string Path { get; set; }
    }
}