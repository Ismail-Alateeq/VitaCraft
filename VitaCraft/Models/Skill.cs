namespace VitaCraft.Models
{
    public class Skill
    {
        public int skillId { get; set; }
        public string? skillName { get; set; }
        public string? skillType { get; set; }
        public int? resumeID { get; set; }
        public Resume? resume { get; set; }
        public int? portFolioId { get; set; }
        public PortFolio? portFolio { get; set; }



    }
}
