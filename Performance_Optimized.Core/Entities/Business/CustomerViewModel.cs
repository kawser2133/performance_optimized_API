using System.ComponentModel.DataAnnotations;

namespace Performance_Optimized.Core.Entities.Business
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }

        public List<PurchaseViewModel> Purchases { get; set; }
    }

    public class CustomerCreateViewModel
    {
        [Required, MinLength(2), MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(10), MaxLength(15)]
        public string PhoneNumber { get; set; }
    }

    public class CustomerUpdateViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(10), MaxLength(15)]
        public string PhoneNumber { get; set; }
    }
}
