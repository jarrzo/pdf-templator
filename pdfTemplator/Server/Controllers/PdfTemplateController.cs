using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Converters;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Models;

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
            return Ok(await _db.PdfTemplates.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (pdfTemplate == null)
                return NotFound();

            return Ok(pdfTemplate);
        }

        [HttpPost("{id}/convert")]
        public async Task<IActionResult> ConvertToPdf([FromRoute] int id, [FromBody] List<PdfKeyValue> data)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (pdfTemplate == null)
                return NotFound();

            _converter.Template = pdfTemplate;
            _converter.Data = data;

            var pdfBase64String = _converter.CreatePdf();

            return Ok(pdfBase64String);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PdfTemplate pdfTemplate)
        {
            _db.PdfTemplates.Add(pdfTemplate);
            await _db.SaveChangesAsync();

            return Ok(true);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(PdfTemplate pdfTemplate, int id)
        {
            var dbPdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPdfTemplate == null)
                return NotFound();

            dbPdfTemplate.Name = pdfTemplate.Name;
            dbPdfTemplate.Content = pdfTemplate.Content;

            await _db.SaveChangesAsync();

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbPdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPdfTemplate == null)
                return NotFound();

            _db.PdfTemplates.Remove(dbPdfTemplate);
            await _db.SaveChangesAsync();

            return Ok(true);
        }
    }
}