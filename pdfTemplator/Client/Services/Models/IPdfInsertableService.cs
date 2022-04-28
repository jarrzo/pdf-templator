using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IPdfInsertableService : IService
    {
        Task<IResult<PdfInsertable>> GetAsync(int id);
        Task<IResult<PdfInsertable>> SaveAsync(PdfInsertable pdfInsertable);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
