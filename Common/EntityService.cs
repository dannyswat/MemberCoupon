using MemberCoupon.Data;
using Microsoft.EntityFrameworkCore;

namespace MemberCoupon.Common
{
    public class EntityService<TEntity> where TEntity : class
    {
        readonly ApplicationDbContext _context;
        public EntityService(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public Task<List<TEntity>> ListAsync(ListItemsModel listModel)
        {
            IQueryable<TEntity> qry = _context.Set<TEntity>();

            Filter(ref qry, listModel?.Filters);
            Sort(ref qry, listModel?.Sort);
            Page(ref qry, listModel);

            return qry.ToListAsync(); ;
        }

        public Task<int> CountAsync(IEnumerable<ListItemsFilter> filters)
        {
            IQueryable<TEntity> qry = _context.Set<TEntity>();

            Filter(ref qry, filters);

            return qry.CountAsync();
        }

        public virtual void Page(ref IQueryable<TEntity> qry, ListItemsModel pager)
        {
            if (pager == null) return;

            qry = qry.Skip(pager.SkipRecordsCount)
                .Take(pager.PageSize);
        }

        public virtual void Filter(ref IQueryable<TEntity> qry, IEnumerable<ListItemsFilter> filters)
        {
            if (filters == null) return;
        }

        public virtual void Sort(ref IQueryable<TEntity> qry, string sort)
        {
        }
    }
}
