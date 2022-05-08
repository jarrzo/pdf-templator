using pdfTemplator.Shared.Constants.Enums;
using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class DataSource : BaseModel
    {
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
        public RequestMethod Method { get; set; }
        public string Url { get; set; } = null!;
        public DataSourceType Type { get; set; }
        public string? HeadersJSON { get; set; }
    }
}
