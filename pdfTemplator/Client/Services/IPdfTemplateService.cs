using pdfTemplator.Shared;

namespace pdfTemplator.Client.Services
{
    public interface IPdfTemplateService
    {
        event Action OnChange;
        List<PdfTemplate> PdfTemplates { get; set; }
        Task<List<PdfTemplate>> GetPdfTemplates();
        Task<PdfTemplate> GetPdfTemplate(int id);
        Task<PdfTemplate> CreatePdfTemplate(PdfTemplate pdfTemplate);
        Task<string> ConvertPdfTemplate(int id);
        Task<PdfTemplate> UpdatePdfTemplate(PdfTemplate pdfTemplate, int id);
        Task<List<PdfTemplate>> DeletePdfTemplate(int id);
    }
}
