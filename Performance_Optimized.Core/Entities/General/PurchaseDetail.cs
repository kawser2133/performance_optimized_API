using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Performance_Optimized.Core.Entities.General
{
    [Table("PurchaseDetails")]
    public class PurchaseDetail : BaseEntity<int>
    {
        [Required]
        public int PurchaseId { get; set; }
        [ForeignKey(nameof(PurchaseId))]
        public Purchase Purchase { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }
    }

}
