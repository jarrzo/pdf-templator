using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface ICategoryService : IService
    {
        Task<IResult<List<Category>>> GetAllAsync();
        Task<IResult<Category>> GetAsync(int id);
        Task<IResult<Category>> SaveAsync(Category request);
        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<List<Template>>> GetTemplates(int id);
    }
}
