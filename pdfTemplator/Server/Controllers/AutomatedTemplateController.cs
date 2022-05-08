using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pdfTemplator.Server.Data;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/automatedTemplate")]
    [Authorize]
    public class AutomatedTemplateController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AutomatedTemplateController> _logger;

        public AutomatedTemplateController(ILogger<AutomatedTemplateController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var templates = await _db.AutomatedTemplates.Include(x => x.Template).Include(x => x.DataSource).ToListAsync();
            return Ok(await Result<List<AutomatedTemplate>>.SuccessAsync(templates));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var template = await _db.AutomatedTemplates.Include(x => x.Template).Include(x => x.DataSource).FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
                return Ok(await Result<AutomatedTemplate>.FailAsync("Not found!"));

            return Ok(await Result<AutomatedTemplate>.SuccessAsync(template));
        }

        [HttpPost]
        public async Task<IActionResult> Create(AutomatedTemplate template)
        {
            _db.AutomatedTemplates.Add(template);
            await _db.SaveChangesAsync();

            return Ok(await Result<AutomatedTemplate>.SuccessAsync(template, "Template created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(AutomatedTemplate template, int id)
        {
            var dbTemplate = await _db.AutomatedTemplates.Include(x => x.Template).Include(x => x.DataSource).FirstOrDefaultAsync(x => x.Id == id);

            if (dbTemplate == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            dbTemplate.Name = template.Name;
            dbTemplate.Description = template.Description;
            dbTemplate.TimeParams = template.TimeParams;
            dbTemplate.SavePath = template.SavePath;
            dbTemplate.SendEmail = template.SendEmail;
            dbTemplate.TemplateId = template.TemplateId;
            dbTemplate.DataSourceId = template.DataSourceId;

            _db.Update(dbTemplate);
            await _db.SaveChangesAsync();

            return Ok(await Result<AutomatedTemplate>.SuccessAsync(dbTemplate, "Template updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbTemplate = await _db.AutomatedTemplates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbTemplate == null)
                return Ok(await Result<int>.FailAsync("Not found!"));

            _db.AutomatedTemplates.Remove(dbTemplate);
            await _db.SaveChangesAsync();

            return Ok(await Result<int>.SuccessAsync(dbTemplate.Id, "Template deleted"));
        }
    }
}