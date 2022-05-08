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
    [Route("api/template")]
    [Authorize]
    public class TemplateController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<TemplateController> _logger;
        private readonly HtmlToPdfConverter _converter;

        public TemplateController(ILogger<TemplateController> logger, ApplicationDbContext db, HtmlToPdfConverter converter)
        {
            _logger = logger;
            _db = db;
            _converter = converter;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var templates = await _db.Templates.Include(x => x.Category).ToListAsync();
            return Ok(await Result<List<Template>>.SuccessAsync(templates));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var template = await _db.Templates.Include(x => x.Category).Include(x => x.Fields).FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
                return Ok(await Result<Template>.FailAsync("Not found!"));

            return Ok(await Result<Template>.SuccessAsync(template));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Template template)
        {
            _db.Templates.Add(template);
            await _db.SaveChangesAsync();

            return Ok(await Result<Template>.SuccessAsync(template, "Template created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Template template, int id)
        {
            var dbTemplate = await _db.Templates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbTemplate == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            dbTemplate.Name = template.Name;
            dbTemplate.Description = template.Description;
            dbTemplate.Content = template.Content;
            dbTemplate.CategoryId = template.CategoryId;

            _db.Update(dbTemplate);
            await _db.SaveChangesAsync();

            return Ok(await Result<Template>.SuccessAsync(dbTemplate, "Template updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbTemplate = await _db.Templates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbTemplate == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            _db.Templates.Remove(dbTemplate);
            await _db.SaveChangesAsync();

            return Ok(await Result<int>.SuccessAsync(dbTemplate.Id, "Template deleted"));
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

            var conversion = _converter.CreatePdf();
            var pdfBase64String = HtmlToPdfConverter.GetEncodedContents(conversion);

            return Ok(await Result<string>.SuccessAsync(pdfBase64String, "Template converted"));
        }

        [HttpGet("{id}/conversions")]
        public async Task<IActionResult> GetConversions(int id)
        {
            var template = await _db.Templates.FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
                return Ok(await Result<Template>.FailAsync("Template not found!"));

            var conversions = _db.Conversions.Where(x => x.TemplateId == id).ToList() ?? new();

            return Ok(await Result<List<Conversion>>.SuccessAsync(conversions));
        }
    }
}