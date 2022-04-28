using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Converters;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/pdfConversion")]
    public class PdfConversionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PdfConversionsController> _logger;

        public PdfConversionsController(ILogger<PdfConversionsController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pdfConversions = await _db.PdfConversions.ToListAsync();
            return Ok(await Result<List<PdfConversion>>.SuccessAsync(pdfConversions));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pdfConversion = await _db.PdfConversions.FirstOrDefaultAsync(x => x.Id == id);

            if (pdfConversion == null)
                return Ok(await Result<PdfConversion>.FailAsync("Not found!"));

            return Ok(await Result<PdfConversion>.SuccessAsync(pdfConversion));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PdfConversion pdfConversion)
        {
            _db.PdfConversions.Add(pdfConversion);
            await _db.SaveChangesAsync();

            return Ok(await Result<PdfConversion>.SuccessAsync(pdfConversion, "Template created"));
        }
    }
}