using VitaCraft.Models.DTOs;

namespace VitaCraft.Interfaces
{
    public interface IPortFolioOpenAiService
    {
        Task<PortfolioJsonDto> ParsePortFolioAsync(PortFolioDTO portFolioRowData);

    }
}
