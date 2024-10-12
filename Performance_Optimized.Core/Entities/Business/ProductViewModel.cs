using System.ComponentModel.DataAnnotations;

namespace Performance_Optimized.Core.Entities.Business
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }

    public class ProductCreateViewModel
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

    public class ProductUpdateViewModel
    {
        [Required]
        public int Id { get; set; }
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
