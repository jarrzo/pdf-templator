using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IPdfInsertableService : IService
    {
        Task<IResult<List<PdfInsertableDto>>> GetPdfInsertables(PdfTemplateDto pdfTemplate);
        Task<IResult<PdfInsertableDto>> GetPdfInsertable(PdfTemplateDto pdfTemplate, int pdfInsertableId);
        Task<IResult<PdfInsertableDto>> SaveAsync(PdfTemplateDto pdfTemplate, PdfInsertableDto pdfInsertable);
        Task<IResult<int>> DeleteAsync(PdfTemplateDto pdfTemplate, int pdfInsertableId);
    }
}
