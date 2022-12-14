using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OronaMVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(1, 100)]
        public double Price { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        [Required]
        public int CleaningTypeId { get; set; }
        public CleaningType? CleaningType { get; set; }

        [Required]
        public int WindowTypeId { get; set; }
        public WindowType? WindowType { get; set; }
    }
}
