namespace VitaCraft.Models.DTOs
{
    public class ResumeJsonDto
    {

        // PersonalInfo
        public int resumeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? CreatedDate { get; set; }
        public string? ModifiedDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Summary { get; set; }
        public string? Title { get; set; }
        public string? GitHubLink { get; set; }
        public string? LinkedinLink { get; set; }
        public List<EducationItem>? Educations { get; set; }
        public List<ExperienceItem>? Experiences { get; set; }
        public List<SkillItem1>? Skills { get; set; }
        public List<CertificateItem>? Certificates { get; set; }
        public string EndUserId { get; set; }

        public int CompletionPercentage
        {
            get
            {
                int totalFields = 10;
                int filledFields = 0;

                if (!string.IsNullOrWhiteSpace(FirstName)) filledFields++;
                if (!string.IsNullOrWhiteSpace(LastName)) filledFields++;
                if (!string.IsNullOrWhiteSpace(Email)) filledFields++;
                if (!string.IsNullOrWhiteSpace(PhoneNumber)) filledFields++;
                if (!string.IsNullOrWhiteSpace(Title)) filledFields++;
                if (!string.IsNullOrWhiteSpace(Summary)) filledFields++;
                if (!string.IsNullOrWhiteSpace(GitHubLink)) filledFields++;
                if (!string.IsNullOrWhiteSpace(LinkedinLink)) filledFields++;
                if (Educations != null && Educations.Any()) filledFields++;
                if (Experiences != null && Experiences.Any()) filledFields++;
                if (Skills != null && Skills.Any()) filledFields++;

                return (int)Math.Round((double)filledFields / totalFields * 100);
            }
        }


    }

    public class EducationItem
    {
        public string? CollegeName { get; set; }
        public string? DegreeType { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Major { get; set; }
        public double? GPA { get; set; }
    }

    public class ExperienceItem
    {
        public string? Title { get; set; }
        public string? Company { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool? IsCurrent { get; set; }
        public string? Duties { get; set; }
    }

    public class SkillItem1
    {
        public string? SkillName { get; set; }
        public string? SkillType { get; set; }
    }

    public class CertificateItem
    {
        public string Title { get; set; }
        public string? ProviderName { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Field { get; set; }
        public double? GPA { get; set; }
    }


}

