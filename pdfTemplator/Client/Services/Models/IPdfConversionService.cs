using pdfTemplator.Client.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IPdfConversionService : IService
    {
        Task<IResult<List<PdfConversion>>> GetAllAsync(PdfTemplate pdfTemplate);
        Task<IResult<PdfConversion>> GetAsync(PdfTemplate pdfTemplate, int pdfConversionid);
        Task<IResult<PdfConversion>> SaveAsync(PdfTemplate pdfTemplate, int pdfConversionid);
        Task<IResult<PdfConversion>> ConvertAsync(PdfTemplate pdfTemplate, PdfConversion pdfConversion);
        Task<IResult<int>> DeleteAsync(PdfTemplate pdfTemplate, int pdfInsertableId);
    }
}
