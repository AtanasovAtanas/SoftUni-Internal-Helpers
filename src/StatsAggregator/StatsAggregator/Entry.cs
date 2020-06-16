namespace StatsAggregator
{
    using System;

    public class Entry
    {
        public Entry() { }

        public Entry(string email, string url, string region, DateTime createdOn)
        {
            Email = email;
            Url = url;
            Region = region;
            CreatedOn = createdOn;
        }

        public string Email { get; set; }

        public string Url { get; set; }

        public string Region { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}