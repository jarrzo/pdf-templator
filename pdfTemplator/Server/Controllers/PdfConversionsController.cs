using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Converters;
using pdfTemplator.Server.Data;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/pdfConversion")]
    public class PdfConversionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PdfConversionsController> _logger;
        private readonly HtmlToPdfConverter _converter;

        public PdfConversionsController(ILogger<PdfConversionsController> logger, ApplicationDbContext db, HtmlToPdfConverter converter)
        {
            _logger = logger;
            _db = db;
            _converter = converter;
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
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == pdfConversion.PdfTemplateId);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            _db.PdfConversions.Add(pdfConversion);
            await _db.SaveChangesAsync();

            return Ok(await Result<PdfConversion>.SuccessAsync(pdfConversion, "Conversion created"));
        }

        [HttpPost("{id}/convert")]
        public async Task<IActionResult> ConvertToPdf([FromRoute] int id, [FromBody] List<PdfKeyValue> data)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (pdfTemplate == null)
                return Ok(await Result<string>.FailAsync("Not found!"));

            _converter.Template = pdfTemplate;
            _converter.Data = data;

            var pdfBase64String = _converter.CreatePdf();

            return Ok(await Result<string>.SuccessAsync(pdfBase64String, "Template converted"));
        }
    }
}