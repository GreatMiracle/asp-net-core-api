﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs.Request
{
    public class ImageUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string FileName { get; set; }

        public string? FileDescription { get; set; }
    }
}
