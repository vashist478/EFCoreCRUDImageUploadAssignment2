using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreImageUploadAssignment2.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ImageName { get; set; }

        [Column(TypeName = "nvarchar(400)")]
        public string ImagePath { get; set; }

        [NotMapped]
        [DisplayName("Upload file")]
        public IFormFile ImageFile { get; set; }
    }
}
