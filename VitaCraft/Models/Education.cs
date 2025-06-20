namespace VitaCraft.Models
{
    public class Education
    {
        public int educationId { get; set; }
        public string? collageName { get; set; }
        public string? degreeType { get;set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? major { get; set; }
        public double? GPA { get; set; }
        public int resumeID { get; set; }
        public Resume  resume { get; set; }




    }
}
