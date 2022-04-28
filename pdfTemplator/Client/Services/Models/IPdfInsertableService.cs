using pdfTemplator.Client.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IPdfInsertableService : IService
    {
        Task<IResult<List<PdfInsertable>>> GetAllAsync(PdfTemplate pdfTemplate);
        Task<IResult<PdfInsertable>> GetAsync(PdfTemplate pdfTemplate, int pdfInsertableId);
        Task<IResult<PdfInsertable>> SaveAsync(PdfTemplate pdfTemplate, PdfInsertable pdfInsertable);
        Task<IResult<int>> DeleteAsync(PdfTemplate pdfTemplate, int pdfInsertableId);
    }
}
