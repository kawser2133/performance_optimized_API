using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Performance_Optimized.Core.Entities.General
{
    [Table("Products")]
    public class Product : BaseEntity<int>
    {
        [Required, MinLength(4), MaxLength(100)]
        public string Name { get; set; }
        [Required, MinLength(2), MaxLength(8)]
        public string Code { get; set; }
        public string? Description { get; set; }
        [Required]
        public float Price { get; set; }
        public bool IsActive { get; set; }
    }
}
