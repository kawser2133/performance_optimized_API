using System.ComponentModel.DataAnnotations;

namespace Performance_Optimized.Core.Entities.Business
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public CustomerViewModel Customer { get; set; }
        public List<PurchaseDetailViewModel> PurchaseDetails { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }

    public class PurchaseDetailViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }

    public class PurchaseCreateViewModel
    {
        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public List<PurchaseDetailCreateViewModel> PurchaseDetails { get; set; }
    }

    public class PurchaseDetailCreateViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

    }

    public class PurchaseUpdateViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public List<PurchaseDetailUpdateViewModel> PurchaseDetails { get; set; }
    }

    public class PurchaseDetailUpdateViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
