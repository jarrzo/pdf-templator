using iText.Html2pdf;
using Microsoft.Extensions.Options;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Models;
using System.Text.Json;

namespace pdfTemplator.Server.Converters
{
    public class HtmlToPdfConverter
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HtmlToPdfConverter> _logger;
        private readonly PathsOptions _paths;
        private readonly ConverterProperties _converterProperties;
        public PdfTemplate? Template;
        public List<PdfKeyValue>? Data;
        private string? _pdfPath;
        private string? _pdfName;

        public HtmlToPdfConverter() { }

        public HtmlToPdfConverter(ILogger<HtmlToPdfConverter> logger, IOptions<PathsOptions> options, ApplicationDbContext db)
        {
            _logger = logger;
            _paths = options.Value;
            _db = db;

            _converterProperties = new();
        }

        public string CreatePdf()
        {
            if (Template == null) return "";

            FillTemplate();

            GenerateFileName();
            CreateDir();

            FileStream pdf = File.Open(_pdfPath + _pdfName, FileMode.Create);
            try
            {
                HtmlConverter.ConvertToPdf(Template.Content, pdf, _converterProperties);
                _logger.LogInformation("Converted file: {fileName}", _pdfName);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Convertation failed: {error}", ex.Message);
                throw;
            }
            pdf.Close();

            CreatePdfConversion();

            return Convert.ToBase64String(File.ReadAllBytes(_pdfPath + _pdfName));
        }

        public void FillTemplate()
        {
            if (Data != null)
            {
                foreach (var item in Data)
                {
                    var key = "{{" + item.Key + "}}";
                    var value = item.Value;
                    Template.Content = Template.Content.Replace(key, value);
                }
            }
        }

        private void GenerateFileName()
        {
            _pdfName = Path.GetRandomFileName() + ".pdf";
        }

        private void CreateDir()
        {
            _pdfPath = Directory.CreateDirectory(_paths.PdfStoringPath + DateTime.Now.ToString("\\\\yyyy\\\\MM\\\\dd\\")).FullName;
        }

        private void CreatePdfConversion()
        {
            _db.PdfConversions.Add(new PdfConversion
            {
                DataJSON = JsonSerializer.Serialize(Data),
                PdfTemplate = Template!,
                PdfPath = _pdfPath + _pdfName,
            });
            _db.SaveChanges();
        }
    }
}
