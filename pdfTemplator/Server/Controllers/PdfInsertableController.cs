using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Converters;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/pdfTemplate/{pdfTemplateId}/pdfInsertable")]
    public class PdfInsertableController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PdfInsertableController> _logger;

        public PdfInsertableController(ILogger<PdfInsertableController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pdfTemplateId)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == pdfTemplateId);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            var pdfInsertables = pdfTemplate.Insertables?.ToList() ?? new();
            return Ok(await Result<List<PdfInsertable>>.SuccessAsync(pdfInsertables));
        }

        [HttpGet("{pdfInsertableId}")]
        public async Task<IActionResult> Get(int pdfTemplateId, int pdfInsertableId)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == pdfTemplateId);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            var pdfInsertable = pdfTemplate.Insertables?.FirstOrDefault(x => x.Id == pdfInsertableId);

            if (pdfInsertable == null)
                return Ok(await Result<PdfInsertable>.FailAsync("Insertable not found!"));

            return Ok(await Result<PdfInsertable>.SuccessAsync(pdfInsertable));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PdfInsertable pdfInsertable, int pdfTemplateId)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == pdfTemplateId);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            _db.PdfInsertables.Add(pdfInsertable);
            await _db.SaveChangesAsync();

            return Ok(await Result<PdfInsertable>.SuccessAsync(pdfInsertable, "Insertable created"));
        }

        [HttpPut("{pdfInsertableId}")]
        public async Task<IActionResult> Update(PdfInsertable pdfInsertable, int pdfTemplateId, int pdfInsertableId)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == pdfTemplateId);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            var dbPdfInsertable = pdfTemplate.Insertables?.FirstOrDefault(x => x.Id == pdfInsertableId);

            if (dbPdfInsertable == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            dbPdfInsertable.Key = pdfInsertable.Key;
            dbPdfInsertable.Type = pdfInsertable.Type;

            _db.Update(dbPdfInsertable);
            await _db.SaveChangesAsync();

            return Ok(await Result<PdfInsertable>.SuccessAsync(dbPdfInsertable, "Insertable updated"));
        }

        [HttpDelete("{pdfInsertableId}")]
        public async Task<IActionResult> Delete(int pdfTemplateId, int pdfInsertableId)
        {

            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == pdfTemplateId);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            var dbPdfInsertable = pdfTemplate.Insertables?.FirstOrDefault(x => x.Id == pdfInsertableId);

            if (dbPdfInsertable == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            _db.PdfInsertables.Remove(dbPdfInsertable);
            await _db.SaveChangesAsync();

            return Ok(await Result<int>.SuccessAsync(dbPdfInsertable.Id, "Insertable deleted"));
        }
    }
}