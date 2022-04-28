using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IPdfTemplateService : IService
    {
        Task<IResult<List<PdfTemplateDto>>> GetPdfTemplates();
        Task<IResult<PdfTemplateDto>> GetPdfTemplate(int id);
        Task<IResult<int>> SaveAsync(PdfTemplateDto request);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
