using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IPdfTemplateService : IService
    {
        Task<IResult<List<PdfTemplate>>> GetAllAsync();
        Task<IResult<List<PdfTemplate>>> GetAllByCategoryAsync(int categoryId);
        Task<IResult<PdfTemplate>> GetAsync(int id);
        Task<IResult<PdfTemplate>> SaveAsync(PdfTemplate request);
        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<List<PdfInsertable>>> GetPdfInsertablesAsync(int id);
        Task<IResult<List<PdfConversion>>> GetPdfConversionsAsync(int id);
    }
}
