using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class AutomatedTemplate : BaseModel
    {
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
        [Required(AllowEmptyStrings = true), MaxLength(512)]
        public string Description { get; set; } = null!;
        [Required(AllowEmptyStrings = true), MaxLength(64)]
        public string TimeParams { get; set; } = null!;

        [Required(AllowEmptyStrings = true), MaxLength(64)]
        public string SendEmail { get; set; } = null!;
        [Required(AllowEmptyStrings = true), MaxLength(128)]
        public string SavePath { get; set; } = null!;

        public int TemplateId { get; set; }
        public virtual Template? Template { get; set; } = null!;

        public int DataSourceId { get; set; }
        public virtual DataSource? DataSource { get; set; } = null!;
    }
}
