using pdfTemplator.Shared.Constants.Enums;
using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class Field : BaseModel
    {

        [Required, MaxLength(64)]
        public string Key { get; set; } = null!;
        public FieldType Type { get; set; }
        public string ParamsJSON { get; set; } = null!;

        public virtual ICollection<Template>? Templates { get; set; }
    }
}
