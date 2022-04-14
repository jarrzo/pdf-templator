using iText.Html2pdf;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Models;
using pdfTemplator.Shared;

namespace pdfTemplator.Server.Converters
{
    public class HtmlToPdfConverter
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HtmlToPdfConverter> _logger;
        private readonly PathsOptions _paths;
        public PdfTemplate? Template;
        public List<PdfKeyValue>? Data;
        private string? _pdfPath;

        public HtmlToPdfConverter() { }

        public HtmlToPdfConverter(ILogger<HtmlToPdfConverter> logger, IOptions<PathsOptions> options, ApplicationDbContext db)
        {
            _logger = logger;
            _paths = options.Value;
            _db = db;
        }

        public void FillPdf()
        {
            if (Template != null && Data != null)
            {
                foreach (var item in Data)
                {
                    var key = "{{" + item.Key + "}}";
                    var value = item.Value;
                    Template.Content = Template.Content.Replace(key, value);
                }
            }
        }

        public string CreatePdf()
        {
            FillPdf();

            ConverterProperties converterProperties = new ConverterProperties();

            var fileName = Path.GetRandomFileName() + ".pdf";

            var path = _paths.PdfStoringPath + DateTime.Now.ToString("\\\\yyyy\\\\MM\\\\dd") + "\\";
            Directory.CreateDirectory(path);

            FileStream pdf = File.Open(path + fileName, FileMode.Create);

            try
            {
                if (Template != null)
                {
                    HtmlConverter.ConvertToPdf(Template.Content, pdf, converterProperties);
                    _logger.LogInformation("Converted file: {fileName}", fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Convertation failed: {error}", ex.Message);
                throw;
            }

            pdf.Close();

            _pdfPath = pdf.Name;

            CreatePdfConversion();

            return Convert.ToBase64String(File.ReadAllBytes(_pdfPath));
        }

        private void CreatePdfConversion()
        {
            _db.PdfConversions.Add(new PdfConversion
            {
                DataJSON = JsonConvert.SerializeObject(Data),
                PdfTemplate = Template!,
                PdfPath = _pdfPath!,
            });
            _db.SaveChanges();
        }
    }
}
