namespace VitaCraft.Models
{
    public class Experience
    {
        public int experienceId { get; set; }
        public string? Title { get; set; }
        public string?  company { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public bool? isCurrent { get; set; } = false;
        public string? duties { get; set; }
        public int resumeID { get; set; }
        public Resume resume { get; set; }


    }
}
