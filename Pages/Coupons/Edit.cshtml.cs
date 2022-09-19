using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MemberCoupon.Data;
using MemberCoupon.Models;

namespace MemberCoupon.Pages.Coupons
{
    public class EditModel : PageModel
    {
        private readonly MemberCoupon.Data.ApplicationDbContext _context;

        public EditModel(MemberCoupon.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Coupon Coupon { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Coupons == null)
            {
                return NotFound();
            }

            var coupon =  await _context.Coupons.Include(e => e.ExclusiveMemberGroup).FirstOrDefaultAsync(m => m.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            Coupon = coupon;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Coupon.RedeemedCount = _context.Redemptions.Count(e => e.CouponId == Coupon.Id);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponExists(Coupon.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CouponExists(int id)
        {
          return _context.Coupons.Any(e => e.Id == id);
        }
    }
}
