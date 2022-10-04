using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CMS.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string SneakerNum { get; set; }
        [Required]
        public string SneakerImg { get; set; }
        [Required]
        public bool Sale { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,0)")]
        public decimal Price { get; set; }

        // Foreign Key
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
    public class Category
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
