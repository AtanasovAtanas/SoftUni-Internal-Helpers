namespace SubtitlesMerger
{
    using System;

    public static class MyExtensions
    {
        public static string ToSubtitleFormat(this TimeSpan ts)
        {
            var hours = ts.Hours;
            var minutes = ts.Minutes;
            var seconds = ts.Seconds;
            var milliseconds = ts.Milliseconds;

            return $"{hours}:{minutes:D2}:{seconds:D2}.{milliseconds:D3}";
        }
    }
}