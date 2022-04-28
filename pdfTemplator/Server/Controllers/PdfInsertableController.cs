using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Data;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/pdfInsertable")]
    public class PdfInsertableController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PdfInsertableController> _logger;

        public PdfInsertableController(ILogger<PdfInsertableController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pdfInsertable = await _db.PdfInsertables.FirstOrDefaultAsync(x => x.Id == id);

            if (pdfInsertable == null)
                return Ok(await Result<PdfInsertable>.FailAsync("Insertable not found!"));

            return Ok(await Result<PdfInsertable>.SuccessAsync(pdfInsertable));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PdfInsertable pdfInsertable)
        {
            _logger.LogInformation("here");

            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == pdfInsertable.PdfTemplateId);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            _db.PdfInsertables.Add(pdfInsertable);
            await _db.SaveChangesAsync();

            return Ok(await Result<PdfInsertable>.SuccessAsync(pdfInsertable, "Insertable created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(PdfInsertable pdfInsertable, int id)
        {
            var pdfTemplate = await _db.PdfTemplates.FirstOrDefaultAsync(x => x.Id == pdfInsertable.PdfTemplateId);

            if (pdfTemplate == null)
                return Ok(await Result<PdfTemplate>.FailAsync("Template not found!"));

            var dbPdfInsertable = await _db.PdfInsertables.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPdfInsertable == null)
                return Ok(await Result<int>.FailAsync("Insertable not found!"));

            dbPdfInsertable.Key = pdfInsertable.Key;
            dbPdfInsertable.Type = pdfInsertable.Type;

            _db.Update(dbPdfInsertable);
            await _db.SaveChangesAsync();

            return Ok(await Result<PdfInsertable>.SuccessAsync(dbPdfInsertable, "Insertable updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbPdfInsertable = await _db.PdfInsertables.FirstOrDefaultAsync(x => x.Id == id);

            if (dbPdfInsertable == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            _db.PdfInsertables.Remove(dbPdfInsertable);
            await _db.SaveChangesAsync();

            return Ok(await Result<int>.SuccessAsync(dbPdfInsertable.Id, "Insertable deleted"));
        }
    }
}