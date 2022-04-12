using pdfTemplator.Shared;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services
{
    public class PdfTemplateService : IPdfTemplateService
    {
        private readonly HttpClient _httpClient;
        public List<PdfTemplate> PdfTemplates { get; set; } = new();
        public event Action OnChange;

        public PdfTemplateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PdfTemplate>> GetPdfTemplates()
        {
            PdfTemplates = await _httpClient.GetFromJsonAsync<List<PdfTemplate>>($"api/pdfTemplate");
            return PdfTemplates;
        }

        public async Task<PdfTemplate> GetPdfTemplate(int id)
        {
            return await _httpClient.GetFromJsonAsync<PdfTemplate>($"api/pdfTemplate/{id}");
        }

        public async Task<PdfTemplate> CreatePdfTemplate(PdfTemplate pdfTemplate)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/pdfTemplate", pdfTemplate);
            await GetPdfTemplates();
            OnChange.Invoke();
            return await result.Content.ReadFromJsonAsync<PdfTemplate>();
        }

        public async Task<PdfTemplate> UpdatePdfTemplate(PdfTemplate pdfTemplate, int id)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/pdfTemplate/{id}", pdfTemplate);
            OnChange.Invoke();
            return await result.Content.ReadFromJsonAsync<PdfTemplate>();
        }

        public async Task<List<PdfTemplate>> DeletePdfTemplate(int id)
        {
            var result = await _httpClient.DeleteAsync($"api/pdfTemplate/{id}");
            await GetPdfTemplates();
            OnChange.Invoke();
            return PdfTemplates;
        }
    }
}
