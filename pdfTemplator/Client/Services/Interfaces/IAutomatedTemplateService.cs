using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Interfaces
{
    public interface IAutomatedTemplateService : IService
    {
        Task<IResult<List<AutomatedTemplate>>> GetAllAsync();
        Task<IResult<AutomatedTemplate>> GetAsync(int id);
        Task<IResult<AutomatedTemplate>> SaveAsync(AutomatedTemplate request);
        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ConvertAsync(int id);
    }
}
