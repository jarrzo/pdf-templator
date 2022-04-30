using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Converters;
using pdfTemplator.Server.Data;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/chart")]
    [Authorize]
    public class ChartsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PdfTemplateController> _logger;
        private readonly HtmlToPdfConverter _converter;
        private readonly int _numberOfDays = 7;

        public ChartsController(ILogger<PdfTemplateController> logger, ApplicationDbContext db, HtmlToPdfConverter converter)
        {
            _logger = logger;
            _db = db;
            _converter = converter;
        }

        [HttpGet]
        [Route("weeklyConversionsCount")]
        public async Task<IActionResult> GetWeeklyConversionsCount()
        {
            DateTime today = DateTime.Now.Date;
            List<double> counts = new();
            for (int i = _numberOfDays; i >= 0; i--)
            {
                counts.Add(await _db.PdfConversions.Where(x => x.CreatedAt.Date == today.AddDays(-1 * i)).CountAsync());
            }
            return Ok(await Result<List<double>>.SuccessAsync(counts));
        }

        [HttpGet]
        [Route("topPdfTemplates")]
        public async Task<IActionResult> GetTopPdfTemplates()
        {
            List<KeyValuePair<PdfTemplate, int>> pairs = new();
            var topTenConversions = _db.PdfConversions.GroupBy(x => x.PdfTemplateId).Select(x => new { PdfTemplateId = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).Take(5).ToList();
            foreach (var pair in topTenConversions)
            {
                pairs.Add(new KeyValuePair<PdfTemplate, int>(await _db.PdfTemplates.FirstAsync(x => x.Id == pair.PdfTemplateId), pair.Count));
            }
            return Ok(await Result<List<KeyValuePair<PdfTemplate, int>>>.SuccessAsync(pairs));
        }
    }
}