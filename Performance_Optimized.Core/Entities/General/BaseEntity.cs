using System.ComponentModel.DataAnnotations;

namespace Performance_Optimized.Core.Entities.General
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
