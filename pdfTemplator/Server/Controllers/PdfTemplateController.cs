using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/pdfTemplate")]
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
            return base.Ok(await Result<List<Models.PdfTemplate>>.SuccessAsync(pdfTemplates));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (pdfTemplate == null)
                return base.Ok(await Result<Models.PdfTemplate>.FailAsync("Not found!"));

            return base.Ok(await Result<Models.PdfTemplate>.SuccessAsync(pdfTemplate));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.PdfTemplate pdfTemplate)
        {
            _db.PdfTemplates.Add(pdfTemplate);
            await _db.SaveChangesAsync();

            return Ok(await Result<PdfTemplate>.SuccessAsync(pdfTemplate, "Template created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Models.PdfTemplate pdfTemplate, int id)
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
    }
}