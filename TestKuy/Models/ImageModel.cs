using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestKuy.Models
{
    public class ImageModel
    {
        [Key]
        public string ProductId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ProductName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ProductDetail { get; set; }

        [Column(TypeName = "float")]
        public float ProductPrice { get; set; }

        [Column(TypeName = "float")]
        public int CartID { get; set; }

        [Column(TypeName = "float")]
        public int CartQuantity { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
