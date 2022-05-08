using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Interfaces
{
    public interface IConversionService : IService
    {
        Task<IResult<Conversion>> GetAsync(int id);
        Task<IResult<Conversion>> SaveAsync(Conversion conversion);
        Task<IResult<string>> ConvertAsync(int id, dynamic data);
    }
}
