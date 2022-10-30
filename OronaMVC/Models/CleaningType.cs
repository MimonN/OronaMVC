using System.ComponentModel.DataAnnotations;

namespace OronaMVC.Models
{
    public class CleaningType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CleaningName { get; set; }
    }
}
