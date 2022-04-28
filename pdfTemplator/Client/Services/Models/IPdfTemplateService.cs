using pdfTemplator.Client.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IPdfTemplateService : IService
    {
        Task<IResult<List<PdfTemplate>>> GetAllAsync();
        Task<IResult<PdfTemplate>> GetAsync(int id);
        Task<IResult<PdfTemplate>> SaveAsync(PdfTemplate request);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
