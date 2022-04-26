using pdfTemplator.Client.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Managers.Models
{
    public interface IChartManager : IManager
    {
        Task<IResult<List<double>>> GetWeeklyConversionsCount();
    }
}
