using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Performance_Optimized.Core.Entities.General
{
    [Table("Customers")]
    public class Customer : BaseEntity<int>
    {
        [Required, MinLength(2), MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(10), MaxLength(50)]
        public string PhoneNumber { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }

}
