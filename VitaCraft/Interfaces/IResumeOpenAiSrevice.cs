using VitaCraft.Models.DTOs;

namespace VitaCraft.Interfaces
{
    public interface IResumeOpenAiSrevice
    {
        Task<ResumeJsonDto> ParseResumeAsync(ResumeDTO resemeRowData);

    }
}
