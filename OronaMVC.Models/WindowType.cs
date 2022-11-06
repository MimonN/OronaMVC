using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OronaMVC.Models
{
    public class WindowType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string WindowTypeName { get; set; }
        public string? ImageUrl { get; set; }
        public List<Product>? Products { get; set; }

    }
}
