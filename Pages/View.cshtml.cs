using MemberCoupon.Data;
using MemberCoupon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MemberCoupon.Pages
{
    [AllowAnonymous]
    public class ViewModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public ViewModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string MemberId { get; set; }

        public string SecureCode { get; set; }

        public List<Coupon> Coupons { get; set; }

        public string ErrorMessage { get; set; }

        public Organization Setting { get; set; }

        public IActionResult OnGet(string id, string secureCode)
        {
            DateTime now = Constants.CurrentTime();

            var member = context.Members.FirstOrDefault(e => e.Number == id && e.SecureKey == secureCode);
            if (member == null || (member.ActiveUntil.HasValue && member.ActiveUntil.Value.Date < now.Date))
                return RedirectToPage("./Error");

            MemberId = member.Number;
            SecureCode = member.SecureKey;
            Coupons = context.Coupons.Where(e => 
                    e.ShowStart <= now && e.ShowEnd >= now 
                    && (!e.ExclusiveMemberGroupId.HasValue || e.ExclusiveMemberGroupId == member.MemberGroupId))
                .Include(e => e.Redemptions.Where(r => r.MemberId == member.Id)).OrderBy(e => e.Name).ToList();
            Setting = context.Organizations.FirstOrDefault();

            return Page();
        }

        public IActionResult OnPostRedeem(string id, string secureCode, int coupon)
        {
            DateTime now = Constants.CurrentTime();

            var member = context.Members.FirstOrDefault(e => e.Number == id && e.SecureKey == secureCode);
            var couponRow = context.Coupons.FirstOrDefault(e => e.Id == coupon);
            if (member == null || couponRow == null || couponRow.Cancelled
                || (member.ActiveUntil.HasValue && member.ActiveUntil.Value.Date < now.Date)
                || (couponRow.ExclusiveMemberGroupId.HasValue && couponRow.ExclusiveMemberGroupId != member.MemberGroupId))
                return RedirectToPage("./Error");

            var redemption = context.Redemptions.FirstOrDefault(e => e.MemberId == member.Id && e.CouponId == coupon);

            if (redemption != null)
            {
                ErrorMessage = "此換領券已換領";
                return OnGet(id, secureCode);
            }

            try
            {
                context.Redemptions.Add(new Redemption
                {
                    Date = now,
                    CouponId = coupon,
                    MemberId = member.Id
                });
                context.SaveChanges();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return OnGet(id, secureCode);
            }
            return RedirectToPage("./View", new { id, secureCode });
        }
    }
}
