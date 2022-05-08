using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using pdfTemplator.Server.Converters;
using pdfTemplator.Server.Data;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/conversion")]
    [Authorize]
    public class ConversionController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ConversionController> _logger;
        private readonly HtmlToPdfConverter _converter;

        public ConversionController(ILogger<ConversionController> logger, ApplicationDbContext db, HtmlToPdfConverter converter)
        {
            _logger = logger;
            _db = db;
            _converter = converter;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var conversion = await _db.Conversions.FirstOrDefaultAsync(x => x.Id == id);

            if (conversion == null)
                return Ok(await Result<Conversion>.FailAsync("Not found!"));

            return Ok(await Result<Conversion>.SuccessAsync(conversion));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Conversion conversion)
        {
            var template = await _db.Templates.FirstOrDefaultAsync(x => x.Id == conversion.TemplateId);

            if (template == null)
                return Ok(await Result<Template>.FailAsync("Template not found!"));

            _db.Conversions.Add(conversion);
            await _db.SaveChangesAsync();

            return Ok(await Result<Conversion>.SuccessAsync(conversion, "Conversion created"));
        }

        [HttpPost("{id}/convert")]
        public async Task<IActionResult> ConvertToPdf([FromRoute] int id, [FromBody] dynamic data)
        {
            var jObject = JObject.Parse(data.ToString());
            var template = await _db.Templates.Include(x => x.Fields).FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
                return Ok(await Result<string>.FailAsync("Not found!"));

            _converter.Template = template;
            _converter.Data = jObject;

            var pdfBase64String = _converter.CreatePdf();

            return Ok(await Result<string>.SuccessAsync(pdfBase64String, "Template converted"));
        }
    }
}