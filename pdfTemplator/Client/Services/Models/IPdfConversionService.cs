using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Insertables;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IPdfConversionService : IService
    {
        Task<IResult<PdfConversion>> GetAsync(int id);
        Task<IResult<PdfConversion>> SaveAsync(PdfConversion pdfConversion);
        Task<IResult<string>> ConvertAsync(int id, InsertablesData data);
    }
}
