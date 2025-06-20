

using System.ComponentModel.DataAnnotations;

namespace VitaCraft.Models.DTOs
{
    public class PortFolioDTO
    {
        public int portFolioId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string Phone { get; set; }
        public string? Title { get; set; } = "";
       

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        public string? Address { get; set; }


        [Url(ErrorMessage = "Please enter a valid LinkedIn URL.")]
        public string? LinkedIn { get; set; }

        [Url(ErrorMessage = "Please enter a valid GitHub URL.")]
        public string? GitHub { get; set; }

        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImageBase64 { get; set; }
        public string? ProfileImageFileName { get; set; }
        public string? ProfileImageContentType { get; set; }

        [Required(ErrorMessage = "Summary/About Me is required.")]
        public string Summary { get; set; }


        [Required(ErrorMessage = "Services are required.")]
        public string Services { get; set; }

        public List<ProjectDto> Projects { get; set; } = new();

        [Required(ErrorMessage = "Skills are required.")]
        public string Skills { get; set; }

        public int CurrentStep { get; set; }
        public class ProjectDto
        {

            public string Name { get; set; }
            public string Description { get; set; }
            public IFormFile? Image { get; set; }
            public string? ImageBase64 { get; set; }
            public string? ImageFileName { get; set; }
            public string? ImageContentType { get; set; }
            public string? Link { get; set; }
        }


    }
}
