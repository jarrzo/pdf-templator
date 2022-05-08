using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public interface ITemplateService : IService
    {
        Task<IResult<List<Template>>> GetAllAsync();
        Task<IResult<List<Template>>> GetAllByCategoryAsync(int categoryId);
        Task<IResult<Template>> GetAsync(int id);
        Task<IResult<Template>> SaveAsync(Template request);
        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<List<Conversion>>> GetConversionsAsync(int id);
    }
}
