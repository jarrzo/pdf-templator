using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Data;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/conversion")]
    [Authorize]
    public class ConversionController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ConversionController> _logger;

        public ConversionController(ILogger<ConversionController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var conversions = await _db.Conversions.ToListAsync();
            return Ok(await Result<List<Conversion>>.SuccessAsync(conversions));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var conversion = await _db.Conversions.FirstOrDefaultAsync(x => x.Id == id);

            if (conversion == null)
                return Ok(await Result<Conversion>.FailAsync("Not found!"));

            return Ok(await Result<Conversion>.SuccessAsync(conversion));
        }
    }
}