namespace VitaCraft.Models
{
    public class UserProfile
    {

        public string 	title { get; set; }
        public string firstName { get; set; }
        public string? secondName { get; set; }
        public string? thirdName { get; set; }
        public string lastName { get; set; }

        public string email { get; set; }
        public string? ImageBase64 { get; set; }
        public string? ImageFileName { get; set; }
        public string? ImageContentType { get; set; }
        public bool? IsOngoing { get; set; }

        public string phoneNumber { get; set; }
        public string? linkedIn { get; set; }
        public string? gitHub { get; set; }
        public string? link3 { get; set; }
        public string? address { get; set; }
        public DateOnly? dateOfBirth { get; set; }
        public string bio { get; set; }
        
    }
}
