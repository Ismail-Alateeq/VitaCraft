namespace VitaCraft.Models
{
    public class Projectt
    {
        public int projecttId { get; set; }
        public string? projectName { get; set; }
        public string? projectDescription { get; set; }
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        
        public string? ImageBase64 { get; set; }
        public string? ImageFileName { get; set; }
        public string? ImageContentType { get; set; }
        public bool? IsOngoing { get; set; }
        public string? projectLink { get; set; }
        public int? portFolioID { get; set; }
        public PortFolio? portFolio { get; set; }


    }

}
