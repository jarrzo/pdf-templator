using pdfTemplator.Server.Converters;
using pdfTemplator.Shared;
using Xunit;

namespace pdfTemplator.Test
{
    public class HtmlToPdfConverterTest
    {
        private readonly HtmlToPdfConverter _converter;

        public HtmlToPdfConverterTest()
        {
            _converter = new HtmlToPdfConverter();
        }

        [Fact]
        public void FillingWithoutKeys()
        {
            string _content = "<h1>Hello</h1>";

            _converter.Template = new PdfTemplate
            {
                Content = _content
            };
            _converter.Data = new();
            _converter.Data.Add(new PdfKeyValue
            {
                Key = "name",
                Value = "test"
            });

            _converter.FillPdf();

            Assert.Equal(_content, _converter.Template.Content);
        }

        [Fact]
        public void FillingWithKeys()
        {
            string _content = "<h1>Hello {{name}}</h1>";

            _converter.Template = new PdfTemplate
            {
                Content = _content
            };
            _converter.Data = new();
            _converter.Data.Add(new PdfKeyValue
            {
                Key = "name",
                Value = "test"
            });

            _converter.FillPdf();

            Assert.NotEqual(_content, _converter.Template.Content);
            Assert.Equal("<h1>Hello test</h1>", _converter.Template.Content);
        }
    }
}