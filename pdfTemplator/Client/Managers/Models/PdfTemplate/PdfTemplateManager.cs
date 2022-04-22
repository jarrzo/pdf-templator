using pdfTemplator.Client.Models;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Managers.Models.PdfTemplate
{
    public class PdfTemplateManager : IPdfTemplateManager
    {
        //private readonly HttpClient _httpClient;

        //public PdfTemplateManager(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        //public async Task<List<PdfTemplate>> GetPdfTemplates()
        //{
        //    return await _httpClient.GetFromJsonAsync<List<PdfTemplate>>($"api/PdfTemplate");
        //}

        //public async Task<PdfTemplate> GetPdfTemplate(int id)
        //{
        //    return await _httpClient.GetFromJsonAsync<PdfTemplate>($"api/PdfTemplate/{id}");
        //}

        //public async Task<bool> CreatePdfTemplate(PdfTemplate pdfTemplate)
        //{
        //    var result = await _httpClient.PostAsJsonAsync($"api/PdfTemplate", pdfTemplate);
        //    return await result.Content.ReadFromJsonAsync<bool>();
        //}

        //public async Task<bool> UpdatePdfTemplate(PdfTemplate pdfTemplate, int id)
        //{
        //    var result = await _httpClient.PutAsJsonAsync($"api/PdfTemplate/{id}", pdfTemplate);
        //    return await result.Content.ReadFromJsonAsync<bool>();
        //}

        //public async Task<bool> DeletePdfTemplate(int id)
        //{
        //    var result = await _httpClient.DeleteAsync($"api/PdfTemplate/{id}");
        //    return await result.Content.ReadFromJsonAsync<bool>();
        //}

        //public async Task<string> ConvertPdfTemplate(int id)
        //{
        //    var result = await _httpClient.PostAsJsonAsync($"api/PdfTemplate/{id}/convert", id);
        //    return result.Content.ToString() ?? "";
        //}
    }
}
