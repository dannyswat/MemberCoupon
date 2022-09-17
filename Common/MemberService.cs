using MemberCoupon.Data;
using MemberCoupon.Models;
using Microsoft.EntityFrameworkCore;

namespace MemberCoupon.Common
{
    public class MemberService : EntityService<Member>
    {
        public MemberService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override void Filter(ref IQueryable<Member> qry, IEnumerable<ListItemsFilter> filters)
        {
            qry = qry.Include(e => e.MemberGroup);

            if (filters == null) return;

            foreach (var filter in filters)
            {
                switch (filter.Property)
                {
                    case nameof(Member.Number):
                        qry = qry.Where(e => e.Number.Contains(filter.Value.ToString()));
                        break;
                    case nameof(Member.MemberGroupId):
                        qry = qry.Where(e => e.MemberGroupId == int.Parse(filter.Value.ToString()));
                        break;
                }
            }
        }
    }
}
