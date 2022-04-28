using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IPdfTemplateService : IService
    {
        Task<IResult<List<PdfTemplate>>> GetPdfTemplates();
        Task<IResult<PdfTemplate>> GetPdfTemplate(int id);
        Task<IResult<int>> SaveAsync(PdfTemplate request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<string>> ConvertAsync(int id);
    }
}
