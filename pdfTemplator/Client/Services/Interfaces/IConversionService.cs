using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Interfaces
{
    public interface IConversionService : IService
    {
        Task<IResult<List<Conversion>>> GetAllAsync();
        Task<IResult<Conversion>> GetAsync(int id);
    }
}
