namespace SubtitlesMerger
{
    using System;

    public class Subtitle
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Content { get; set; }

        public override string ToString()
        {
            return $"{this.StartTime.ToSubtitleFormat()},{this.EndTime.ToSubtitleFormat()}" +
                   Environment.NewLine +
                   this.Content +
                   Environment.NewLine;
        }
    }
}