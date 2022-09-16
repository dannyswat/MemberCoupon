using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MemberCoupon.Data;
using MemberCoupon.Models;

namespace MemberCoupon.Pages.Coupons
{
    public class CreateModel : PageModel
    {
        private readonly MemberCoupon.Data.ApplicationDbContext _context;

        public CreateModel(MemberCoupon.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Coupon Coupon { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Coupons.Add(Coupon);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
