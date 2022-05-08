using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Interfaces
{
    public interface IDataSourceService : IService
    {
        Task<IResult<List<DataSource>>> GetAllAsync();
        Task<IResult<DataSource>> GetAsync(int id);
        Task<IResult<DataSource>> SaveAsync(DataSource request);
        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> GetDataAsync(int id);
    }
}
