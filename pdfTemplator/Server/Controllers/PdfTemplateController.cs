using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Data;
using pdfTemplator.Shared;

namespace pdfTemplator.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PdfTemplateController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PdfTemplateController> _logger;

        public PdfTemplateController(ILogger<PdfTemplateController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
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

        [HttpPost]
        public async Task<IActionResult> Create(PdfTemplate pdfTemplate)
        {
            _db.PdfTemplates.Add(pdfTemplate);
            await _db.SaveChangesAsync();

            return Ok(pdfTemplate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(PdfTemplate pdfTemplate, int id)
        {
            var dbPdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPdfTemplate == null)
                return NotFound();

            dbPdfTemplate.Name = pdfTemplate.Name;
            dbPdfTemplate.Content = pdfTemplate.Content;
            dbPdfTemplate.Insertables = pdfTemplate.Insertables;

            await _db.SaveChangesAsync();

            return Ok(dbPdfTemplate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbPdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPdfTemplate == null)
                return NotFound();

            _db.PdfTemplates.Remove(dbPdfTemplate);
            await _db.SaveChangesAsync();

            return Ok(dbPdfTemplate);
        }
    }
}