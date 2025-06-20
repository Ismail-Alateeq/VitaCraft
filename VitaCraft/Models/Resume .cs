namespace VitaCraft.Models
{
    public class Resume : UserProfile
    {
        public int resumeId { get; set; }
        public string? createdDate { get; set; }
        public string? modifiedDate { get; set; }
        public List<Education> educations { get; set; } 
        public List<Experience> experiences { get; set; }
        public List<Skill> skills { get; set; } 
        public List<Language> languages { get; set; }
        public List<Certificate> certificates { get; set; }
        public EndUser EndUser { get; set; }
        public string EndUserId { get; set; }









    }
}
