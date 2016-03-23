using _DataAccess.UnitOfWork;

namespace _6tactics.Cms.Services.Admin
{
    public abstract class AdminBaseService
    {
        protected IUnitOfWork Uof;

        protected AdminBaseService(IUnitOfWork uof)
        {
            Uof = uof;
        }
    }
}
