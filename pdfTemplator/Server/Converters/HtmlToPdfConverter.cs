using iText.Html2pdf;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using pdfTemplator.Server.Data;
using pdfTemplator.Server.Models;
using pdfTemplator.Shared.Models;
using System.Text.Json;

namespace pdfTemplator.Server.Converters
{
    public class HtmlToPdfConverter
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HtmlToPdfConverter> _logger;
        private readonly PathsOptions _paths;
        private readonly ConverterProperties _converterProperties;
        public Template Template = null!;
        public List<Field> Fields = new();
        public JObject Data = null!;
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

            GetFields();

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

            CreateConversion();

            return Convert.ToBase64String(File.ReadAllBytes(_pdfPath + _pdfName));
        }

        private void GetFields()
        {
            Fields = Template.Fields.ToList();
        }

        private void GenerateFileName()
        {
            _pdfName = Path.GetRandomFileName() + ".pdf";
        }

        private void CreateDir()
        {
            _pdfPath = Directory.CreateDirectory(_paths.PdfStoringPath + DateTime.Now.ToString("\\\\yyyy\\\\MM\\\\dd") + "\\").FullName;
        }

        private void CreateConversion()
        {
            _db.Conversions.Add(new Conversion
            {
                DataJSON = JsonSerializer.Serialize(Data),
                TemplateId = Template!.Id,
                PdfPath = _pdfPath + _pdfName,
            });
            _db.SaveChanges();
        }

        public void FillTemplate()
        {
            _pdfContent = Template.Content.ReplaceLineEndings("");
            if (Data.Count == 0) return;

            Dictionary<string, JArray> arrays = new();
            Dictionary<string, JValue> strings = new();

            foreach (JProperty item in (JToken)Data)
            {
                string key = item.Name;
                JToken value = item.Value;

                if (value.GetType() == typeof(JArray)) arrays.Add(key, (JArray)value);
                if (value.GetType() == typeof(JValue)) strings.Add(key, (JValue)value);
            }

            FillArrays(arrays);
            FillStrings(strings);
        }

        private void FillArrays(Dictionary<string, JArray> arrays)
        {
            foreach (var item in arrays)
                if (Fields.Any(x => x.Key == item.Key))
                    FillObjects(item.Key, item.Value);
        }

        private void FillStrings(Dictionary<string, JValue> strings)
        {
            foreach (var item in strings)
                if (Fields.Any(x => x.Key == item.Key))
                    FillText(item.Key, item.Value);
        }

        private void FillObjects(string key, JArray objects)
        {
            FillSequence(key, objects);
            FillTable(key, objects);
        }

        private void FillSequence(string key, JArray objects)
        {
            string startKey = $"<p>@start_{key}</p>";
            string endKey = $"<p>@end_{key}</p>";

            int start = _pdfContent.IndexOf(startKey);
            if (start == -1) return;
            int end = _pdfContent.IndexOf(endKey, start);
            if (end == -1) return;

            var innerContentRaw = _pdfContent.Substring(start + startKey.Length, end - start - startKey.Length);

            _pdfContent = _pdfContent.Replace(startKey + innerContentRaw + endKey, FillObject(innerContentRaw, objects));
        }

        private void FillTable(string key, JArray objects)
        {
            string startKey = "data-pdffield=\"" + key + "\"";
            string endKey = "</tbody>";

            int startKeyPosition = _pdfContent.IndexOf(startKey);
            if (startKeyPosition == -1) return;
            string afterStartKey = _pdfContent.Substring(startKeyPosition);

            int start = afterStartKey.IndexOf(">");
            int end = afterStartKey.IndexOf(endKey);

            var innerContentRaw = afterStartKey.Substring(start + 1, end - start - 1);

            _pdfContent = _pdfContent.Replace(innerContentRaw, FillObject(innerContentRaw, objects));
        }

        private static string FillObject(string content, JArray objects)
        {
            string preparedContent = "";

            foreach (JToken element in objects)
            {
                var elementContent = content;
                foreach (JProperty prop in element)
                {
                    string propKey = "{{" + prop.Name + "}}";
                    var propValue = prop.Value.ToString();
                    elementContent = elementContent.Replace(propKey, propValue);
                }
                preparedContent += elementContent;
            }

            return preparedContent;
        }

        private void FillText(string key, JValue value)
        {
            key = "{{" + key + "}}";
            _pdfContent = _pdfContent.Replace(key, value.ToString());
        }
    }
}
