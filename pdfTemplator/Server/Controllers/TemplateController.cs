using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public TemplateController(ILogger<TemplateController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
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