using System.ComponentModel.DataAnnotations;

namespace OronaMVC.Models
{
    public class WindowType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string WindowTypeName { get; set; }
        public string? ImageUrl { get; set; }

    }
}
