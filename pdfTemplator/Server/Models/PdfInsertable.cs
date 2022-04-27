﻿using pdfTemplator.Shared.Constants.Enums;
using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Server.Models
{
    public class PdfInsertable : BaseModel
    {
        [Required, MaxLength(64)]
        public string Key { get; set; } = null!;
        public InsertableType Type { get; set; }
    }
}
