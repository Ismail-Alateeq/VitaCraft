namespace VitaCraft.Models
{
    public class Language
    {
        public int languageId { get; set; }
        public string? languageName { get; set; }
        public int? level { get; set;}
        public int resumeID { get; set; }
        public Resume resume { get; set; }


    }
}
