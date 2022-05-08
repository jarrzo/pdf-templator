using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface IFieldService : IService
    {
        Task<IResult<List<Field>>> GetAllAsync();
        Task<IResult<Field>> GetAsync(int id);
        Task<IResult<Field>> SaveAsync(Field field);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
