using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Interfaces
{
    public interface IChartService : IService
    {
        Task<IResult<List<double>>> GetWeeklyConversionsCount();
        Task<IResult<List<KeyValuePair<Template, int>>>> GetTopTemplates();
    }
}
