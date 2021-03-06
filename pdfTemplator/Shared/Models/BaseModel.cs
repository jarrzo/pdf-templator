using Microsoft.EntityFrameworkCore;

namespace pdfTemplator.Shared.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        [Precision(3)]
        public DateTime CreatedAt { get; set; }
        [Precision(3)]
        public DateTime UpdatedAt { get; set; }
    }
}
