using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Data;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/pdfTemplate")]
    [Authorize]
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

        [HttpPost]
        public async Task<IActionResult> Create(PdfTemplate pdfTemplate)
        {
            _db.PdfTemplates.Add(pdfTemplate);
            await _db.SaveChangesAsync();

            return Ok(await Result<PdfTemplate>.SuccessAsync(pdfTemplate, "Template created"));
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

            return Ok(await Result<PdfTemplate>.SuccessAsync(dbPdfTemplate, "Template updated"));
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

        [HttpGet("{id}/pdfInsertables")]
        public async Task<IActionResult> GetPdfInsertables(int id)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            var pdfInsertables = _db.PdfInsertables.Where(x => x.PdfTemplateId == id).ToList() ?? new();
            return Ok(await Result<List<PdfInsertable>>.SuccessAsync(pdfInsertables));
        }

        [HttpGet("{id}/pdfConversions")]
        public async Task<IActionResult> GetPdfConversions(int id)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            var pdfConversions = _db.PdfConversions.Where(x => x.PdfTemplateId == id).ToList() ?? new();

            return Ok(await Result<List<PdfConversion>>.SuccessAsync(pdfConversions));
        }
    }
}