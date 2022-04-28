using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IChartService : IService
    {
        Task<IResult<List<double>>> GetWeeklyConversionsCount();
    }
}
