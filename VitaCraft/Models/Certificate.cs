﻿namespace VitaCraft.Models
{
    public class Certificate
    {
        public int certificateId { get; set; }
        public string? providerName { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? topicName { get; set; }
        public double? GPA { get; set; }
        public int resumeID { get; set; }
        public Resume resume { get; set; }


    }
}
