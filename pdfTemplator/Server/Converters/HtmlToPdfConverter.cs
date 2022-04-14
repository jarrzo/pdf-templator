using iText.Html2pdf;
using pdfTemplator.Server.Models;

namespace pdfTemplator.Server.Converters
{
    public class HtmlToPdfConverter
    {
        private readonly ILogger<HtmlToPdfConverter> _logger;
        private readonly PathsOptions _paths;
        public string Content;

        public HtmlToPdfConverter(ILogger<HtmlToPdfConverter> logger, PathsOptions paths)
        {
            _logger = logger;
            _paths = paths;

            Content = "";
        }

        public FileStream GetPdf()
        {
            ConverterProperties converterProperties = new ConverterProperties();

            var fileName = Guid.NewGuid().ToString() + ".pdf";
            var path = _paths.PdfStoringPath + DateTime.Now.ToString("\\yyyy\\MM\\dd") + "\\";

            FileStream pdf = File.Open(path+fileName, FileMode.Create);

            try
            {
                HtmlConverter.ConvertToPdf(Content, pdf, converterProperties);
                _logger.LogInformation("Converted file: {fileName}", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Convertation failed: {error}", ex.Message);
                throw;
            }

            pdf.Close();

            return pdf;
        }
    }
}
