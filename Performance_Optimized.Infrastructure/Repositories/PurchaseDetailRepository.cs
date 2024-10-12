using Performance_Optimized.Core.Entities.General;
using Performance_Optimized.Core.Interfaces.IRepositories;
using Performance_Optimized.Infrastructure.Data;

namespace Performance_Optimized.Infrastructure.Repositories
{
    public class PurchaseDetailRepository : BaseRepository<PurchaseDetail>, IPurchaseDetailRepository
    {
        public PurchaseDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
