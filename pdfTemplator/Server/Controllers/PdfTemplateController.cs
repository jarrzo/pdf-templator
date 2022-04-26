using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Converters;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfTemplateController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PdfTemplateController> _logger;
        private readonly HtmlToPdfConverter _converter;

        public PdfTemplateController(ILogger<PdfTemplateController> logger, ApplicationDbContext db, HtmlToPdfConverter converter)
        {
            _logger = logger;
            _db = db;
            _converter = converter;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pdfTemplates = await _db.PdfTemplates.ToListAsync();
            return Ok(await Result<List<PdfTemplate>>.SuccessAsync(pdfTemplates));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Not found!"));

            return Ok(await Result<PdfTemplate>.SuccessAsync(pdfTemplate));
        }

        [HttpGet("{id}/conversions")]
        public async Task<IActionResult> GetConversions(int id)
        {
            var pdfTemplate = await _db.PdfTemplates.Include(x => x.Conversions).FirstOrDefaultAsync(x => x.Id == id);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            return Ok(await Result<List<PdfConversion>>.SuccessAsync(pdfTemplate.Conversions));
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

        [HttpPost]
        public async Task<IActionResult> Create(PdfTemplate pdfTemplate)
        {
            _db.PdfTemplates.Add(pdfTemplate);
            await _db.SaveChangesAsync();

            return Ok(await Result<int>.SuccessAsync(pdfTemplate.Id, "Template created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(PdfTemplate pdfTemplate, int id)
        {
            var dbPdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPdfTemplate == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            dbPdfTemplate.Name = pdfTemplate.Name;
            dbPdfTemplate.Description = pdfTemplate.Description;
            dbPdfTemplate.Content = pdfTemplate.Content;

            _db.Update(dbPdfTemplate);
            await _db.SaveChangesAsync();

            return Ok(await Result<int>.SuccessAsync(dbPdfTemplate.Id, "Template updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbPdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPdfTemplate == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            _db.PdfTemplates.Remove(dbPdfTemplate);
            await _db.SaveChangesAsync();

            return Ok(await Result<int>.SuccessAsync(dbPdfTemplate.Id, "Template deleted"));
        }
    }
}