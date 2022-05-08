using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Services;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;
using pdfTemplator.Shared.Extensions;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/dataSources")]
    [Authorize]
    public class DataSourceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<DataSourceController> _logger;

        public DataSourceController(ILogger<DataSourceController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dataSources = await _db.DataSources.ToListAsync();
            return Ok(await Result<List<DataSource>>.SuccessAsync(dataSources));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dataSource = await _db.DataSources.FirstOrDefaultAsync(x => x.Id == id);

            if (dataSource == null)
                return Ok(await Result<DataSource>.FailAsync("Not found!"));

            return Ok(await Result<DataSource>.SuccessAsync(dataSource));
        }

        [HttpPost]
        public async Task<IActionResult> Create(DataSource dataSource)
        {
            _db.DataSources.Add(dataSource);
            await _db.SaveChangesAsync();

            return Ok(await Result<DataSource>.SuccessAsync(dataSource, "DataSource created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(DataSource dataSource, int id)
        {
            var dbDataSource = await _db.DataSources.FirstOrDefaultAsync(x => x.Id == id);

            if (dbDataSource == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            dbDataSource.Name = dataSource.Name;
            dbDataSource.Method = dataSource.Method;
            dbDataSource.Url = dataSource.Url;
            dbDataSource.Type = dataSource.Type;
            dbDataSource.HeadersJSON = dataSource.HeadersJSON;

            _db.Update(dbDataSource);
            await _db.SaveChangesAsync();

            return Ok(await Result<DataSource>.SuccessAsync(dbDataSource, "DataSource updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbDataSource = await _db.DataSources.FirstOrDefaultAsync(x => x.Id == id);

            if (dbDataSource == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            _db.DataSources.Remove(dbDataSource);
            await _db.SaveChangesAsync();

            return Ok(await Result<int>.SuccessAsync(dbDataSource.Id, "DataSource deleted"));
        }

        [HttpGet("{id}/data")]
        public async Task<IActionResult> GetData(int id)
        {
            var dataSource = await _db.DataSources.FirstOrDefaultAsync(x => x.Id == id);

            if (dataSource == null)
                return Ok(await Result<string>.FailAsync("Not found!"));

            var dataGetter = await DataGetter.GetData(dataSource);

            if(!dataGetter.Succeeded)
                return Ok(await Result<List<string>>.FailAsync(dataGetter.Messages));

            var dataEncoded = Convert.ToBase64String(await dataGetter.Data.Content.ReadAsByteArrayAsync());
            return Ok(await Result<string>.SuccessAsync(dataEncoded, "Success"));
        }
    }
}