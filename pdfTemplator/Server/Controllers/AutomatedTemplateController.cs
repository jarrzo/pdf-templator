using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using pdfTemplator.Server.Converters;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Services;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;
using System.Net.Mail;

namespace pdfTemplator.Server.Controllers
{
    [ApiController]
    [Route("api/automatedTemplate")]
    [Authorize]
    public class AutomatedTemplateController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AutomatedTemplateController> _logger;
        private readonly HtmlToPdfConverter _converter;
        private readonly SmtpClient _smtpClient;

        public AutomatedTemplateController(ILogger<AutomatedTemplateController> logger, ApplicationDbContext db, HtmlToPdfConverter converter, SmtpClient smtpClient)
        {
            _logger = logger;
            _db = db;
            _converter = converter;
            _smtpClient = smtpClient;
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

        [HttpGet("{id}/convert")]
        public async Task<IActionResult> Convert(int id)
        {
            var template = await _db.AutomatedTemplates.Include(x => x.Template).Include(x => x.DataSource).FirstOrDefaultAsync(x => x.Id == id);

            if (template == null)
                return Ok(await Result<string>.FailAsync("Not found!"));

            _converter.Template = template.Template!;

            var dataGetter = await DataGetter.GetData(template.DataSource!);

            if (!dataGetter.Succeeded)
                return Ok(await Result<List<string>>.FailAsync(dataGetter.Messages));

            _converter.Data = JObject.Parse(await dataGetter.Data.Content.ReadAsStringAsync());

            var conversion = _converter.CreatePdf();

            if (template.SendEmail != null) SendMail(template, conversion);

            var pdfBase64String = HtmlToPdfConverter.GetEncodedContents(conversion);

            return Ok(await Result<string>.SuccessAsync(pdfBase64String, "Template converted"));
        }

        private void SendMail(AutomatedTemplate template, Conversion conversion)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("example@test.com", "Example");
            mailMessage.To.Add(new MailAddress(template.SendEmail));
            mailMessage.Subject = template.Name;
            mailMessage.Body = "Automated PDF conversion";
            mailMessage.Attachments.Add(new Attachment(conversion.PdfPath, "application/pdf"));
            _smtpClient.Send(mailMessage);
        }
    }
}