namespace VitaCraft.Models
{
    public class Service
    {
        public int serviceId { get; set; }
        public string? serviceName { get; set; }
        public string? serviceDescription { get; set; }
        public int portFolioID { get; set; }
        public PortFolio portFolio { get; set; }



    }
}
