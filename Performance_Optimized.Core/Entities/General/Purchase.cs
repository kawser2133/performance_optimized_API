using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Performance_Optimized.Core.Entities.General
{
    [Table("Purchases")]
    public class Purchase : BaseEntity<int>
    {
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }

        public ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }


}
