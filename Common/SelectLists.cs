using MemberCoupon.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MemberCoupon.Common
{
    public class SelectLists
    {
        readonly ApplicationDbContext _context;
        public SelectLists(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public SelectList MemberGroupList() => new SelectList(
                _context.MemberGroups
                    .Where(r => r.IsDisabled == false)
                    .OrderBy(r => r.Name)
                    .Select(r => new SelectListItem(
                        r.Name,
                        r.Id.ToString())), "Value", "Text");
    }

    public static class NullableSelectList
    {
        static readonly SelectListItem EmptyItem = new SelectListItem("", "");
        static readonly List<SelectListItem> NullableList = new List<SelectListItem>
        { EmptyItem };

        public static SelectList Nullable(this IEnumerable<SelectListItem> items)
        {
            return new SelectList(NullableList.Concat(items), "Value", "Text");
        }
    }
}
