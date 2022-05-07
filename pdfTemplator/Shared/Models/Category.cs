﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pdfTemplator.Shared.Models
{
    public class Category : BaseModel
    {
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
    }
}
