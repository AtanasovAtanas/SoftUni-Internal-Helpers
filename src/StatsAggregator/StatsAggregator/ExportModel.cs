namespace StatsAggregator
{
    using System;

    public class ExportModel
    {
        public ExportModel() { }

        public ExportModel(string username, string regionCode, int courseInstanceId, DateTime createdOn)
        {
            Username = username;
            RegionCode = regionCode;
            CourseInstanceId = courseInstanceId;
            CreatedOn = createdOn;
        }

        public string Username { get; set; }

        public string RegionCode { get; set; }

        public int CourseInstanceId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}