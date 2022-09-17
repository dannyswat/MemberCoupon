using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MemberCoupon.Data;
using MemberCoupon.Models;

namespace MemberCoupon.Pages.Coupons
{
    public class DetailsModel : PageModel
    {
        private readonly MemberCoupon.Data.ApplicationDbContext _context;

        public DetailsModel(MemberCoupon.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Coupon Coupon { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Coupons == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupons.Include(e => e.ExclusiveMemberGroup).FirstOrDefaultAsync(m => m.Id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            else 
            {
                Coupon = coupon;
            }
            return Page();
        }
    }
}
