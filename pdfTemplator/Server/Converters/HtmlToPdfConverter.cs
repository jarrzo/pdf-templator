using iText.Html2pdf;
using Microsoft.Extensions.Options;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Models;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Insertables;
using System.Text.Json;

namespace pdfTemplator.Server.Converters
{
    public class HtmlToPdfConverter
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HtmlToPdfConverter> _logger;
        private readonly PathsOptions _paths;
        private readonly ConverterProperties _converterProperties;
        public PdfTemplate Template = null!;
        public InsertablesData? Data;
        private string _pdfPath = null!;
        private string _pdfName = null!;
        private string _pdfContent = null!;

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
                HtmlConverter.ConvertToPdf(_pdfContent, pdf, _converterProperties);
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

        private void GenerateFileName()
        {
            _pdfName = Path.GetRandomFileName() + ".pdf";
        }

        private void CreateDir()
        {
            _pdfPath = Directory.CreateDirectory(_paths.PdfStoringPath + DateTime.Now.ToString("\\\\yyyy\\\\MM\\\\dd") + "\\").FullName;
        }

        private void CreatePdfConversion()
        {
            _db.PdfConversions.Add(new PdfConversion
            {
                DataJSON = JsonSerializer.Serialize<InsertablesData>(Data),
                PdfTemplateId = Template!.Id,
                PdfPath = _pdfPath + _pdfName,
            });
            _db.SaveChanges();
        }

        public void FillTemplate()
        {
            _pdfContent = Template.Content.ReplaceLineEndings("");
            _logger.LogInformation(_pdfContent);
            if (Data != null)
            {
                FillSequences();
                FillTables();
                FillTexts();
            }
        }

        private void FillSequences()
        {
            foreach (var sequence in Data!.SequenceFields)
            {
                string preparedContent = "";
                string startKey = $"<p>@start_{sequence.Key}</p>";
                string endKey = $"<p>@end_{sequence.Key}</p>";

                int start = _pdfContent.IndexOf(startKey);
                if (start == -1) continue;
                int end = _pdfContent.IndexOf(endKey, start);
                if (end == -1) continue;

                var innerContentRaw = _pdfContent.Substring(start + startKey.Length, end - start - startKey.Length);

                foreach (var element in sequence.Elements)
                {
                    var elementContent = innerContentRaw;
                    foreach(var elementField in element)
                    {
                        var key = "{{" + elementField.Key + "}}";
                        var value = elementField.Value;
                        elementContent = elementContent.Replace(key, value);
                    }
                    preparedContent += elementContent;
                }

                _pdfContent = _pdfContent.Replace(startKey + innerContentRaw + endKey, preparedContent);
            }
        }

        private void FillTables()
        {
            foreach (var table in Data!.TableFields)
            {
                string preparedContent = "";

                string startKey = "data-pdfinsertable=\"" + table.Key + "\"";
                string endKey = "</tbody>";

                int startKeyPosition = _pdfContent.IndexOf(startKey);
                if (startKeyPosition == -1) continue;
                string afterStartKey = _pdfContent.Substring(startKeyPosition);

                int start = afterStartKey.IndexOf(">");
                int end = afterStartKey.IndexOf(endKey);

                var innerContentRaw = afterStartKey.Substring(start + 1, end - start - 1);

                foreach (var element in table.Elements)
                {
                    var elementContent = innerContentRaw;
                    foreach (var elementField in element)
                    {
                        var key = "{{" + elementField.Key + "}}";
                        var value = elementField.Value;
                        elementContent = elementContent.Replace(key, value);
                    }
                    preparedContent += elementContent;
                }

                _pdfContent = _pdfContent.Replace(innerContentRaw, preparedContent);
            }
        }

        private void FillTexts()
        {
            foreach (var textFields in Data!.TextFields)
            {
                var key = "{{" + textFields.Key + "}}";
                var value = textFields.Value;
                _pdfContent = _pdfContent.Replace(key, value);
            }
        }
    }
}
