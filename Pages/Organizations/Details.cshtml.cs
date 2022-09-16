using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MemberCoupon.Data;
using MemberCoupon.Models;

namespace MemberCoupon.Pages.Organizations
{
    public class DetailsModel : PageModel
    {
        private readonly MemberCoupon.Data.ApplicationDbContext _context;

        public DetailsModel(MemberCoupon.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Organization Organization { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var organization = await _context.Organizations.FirstOrDefaultAsync();
            if (organization == null)
            {
                return NotFound();
            }
            else 
            {
                Organization = organization;
            }
            return Page();
        }
    }
}
