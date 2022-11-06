using System.ComponentModel.DataAnnotations;

namespace OronaMVC.Models
{
    public class CleaningType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Cleaning Type")]
        public string CleaningName { get; set; }

        public List<Product>? Products { get; set; }
    }
}
