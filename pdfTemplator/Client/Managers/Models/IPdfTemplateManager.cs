using pdfTemplator.Client.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Managers.Models
{
    public interface IPdfTemplateManager : IManager
    {
        Task<IResult<List<PdfTemplate>>> GetPdfTemplates();
        Task<IResult<PdfTemplate>> GetPdfTemplate(int id);
        Task<IResult<int>> SaveAsync(PdfTemplate request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<string>> ConvertAsync(int id);
    }
}
