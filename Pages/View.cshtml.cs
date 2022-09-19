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

            Coupons = Coupons.OrderBy(e => e.Redemptions?.Count ?? 0).ThenBy(e => e.Quota.HasValue && e.Quota <= e.RedeemedCount ? 0 : 1).ToList();

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

            if (couponRow.Quota.HasValue && couponRow.Quota < couponRow.RedeemedCount)
                return RedirectToPage("./Error");

            var redemption = context.Redemptions.FirstOrDefault(e => e.MemberId == member.Id && e.CouponId == coupon);

            if (redemption != null)
            {
                ErrorMessage = "此換領券已使用";
                return OnGet(id, secureCode);
            }

            try
            {
                context.Database.BeginTransaction();

                int redeemedCount = context.Redemptions.Count(e => e.CouponId == coupon);
                if (couponRow.Quota.HasValue && couponRow.Quota <= redeemedCount)
                    throw new Exception("此換領券已換完");

                context.Redemptions.Add(new Redemption
                {
                    Date = now,
                    CouponId = coupon,
                    MemberId = member.Id
                });
                couponRow.RedeemedCount = redeemedCount + 1;
                context.SaveChanges();

                context.Database.CommitTransaction();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                try { context.Database.RollbackTransaction(); } catch { }
                return OnGet(id, secureCode);
            }
            return RedirectToPage("./View", new { id, secureCode });
        }
    }
}
