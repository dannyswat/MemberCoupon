using MemberCoupon.Data;
using MemberCoupon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MemberCoupon.Pages.Members
{
    public class PrintModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public PrintModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IList<Member> Members { get; set; } = new List<Member>();
        public void OnGet()
        {
            Members = context.Members.Where(e => !e.ActiveUntil.HasValue || e.ActiveUntil.Value < Constants.CurrentTime()).ToList();
        }
    }
}
