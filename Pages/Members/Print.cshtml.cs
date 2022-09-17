using MemberCoupon.Common;
using MemberCoupon.Data;
using MemberCoupon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MemberCoupon.Pages.Members
{
    public class PrintModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly MemberService memberService;

        public PrintModel(ApplicationDbContext context, MemberService memberService)
        {
            this.context = context;
            this.memberService = memberService;
        }

        public IList<Member> Members { get; set; } = new List<Member>();

        public void OnGet([FromQuery] ListItemsModel listModel, string members = null)
        {
            IQueryable<Member> qry = context.Members
                .Where(e => !e.ActiveUntil.HasValue || e.ActiveUntil.Value < Constants.CurrentTime())
                .OrderBy(e => e.Number);

            if (!string.IsNullOrEmpty(members))
            {
                Members = qry.ToList();

                HashSet<string> memberTable = new HashSet<string>(members.Split(','));
                List<Member> toRemove = new List<Member>();

                foreach (var member in Members)
                    if (!memberTable.Contains(member.Number))
                        toRemove.Add(member);

                foreach (var member in toRemove)
                    Members.Remove(member);
            }
            else if (listModel?.Filters != null && listModel.Filters.Count > 0)
            {
                memberService.Filter(ref qry, listModel.Filters);
                Members = qry.ToList();
            }
            else
                Members = qry.ToList();

        }
    }
}
