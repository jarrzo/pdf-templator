﻿using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Server.Models
{
    public class PdfTemplate : BaseModel
    {
        [Required(AllowEmptyStrings = true), MaxLength(64)]
        public string Name { get; set; } = null!;
        [Required(AllowEmptyStrings = true), MaxLength(512)]
        public string Description { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        public string Content { get; set; } = null!;
        public ICollection<PdfInsertable>? Insertables { get; set; }

        public ICollection<PdfConversion>? Conversions { get; set; }
    }
}
