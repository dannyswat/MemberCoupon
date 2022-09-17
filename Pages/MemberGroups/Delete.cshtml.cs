using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MemberCoupon.Data;
using MemberCoupon.Models;

namespace MemberCoupon.Pages.MemberGroups
{
    public class DeleteModel : PageModel
    {
        private readonly MemberCoupon.Data.ApplicationDbContext _context;

        public DeleteModel(MemberCoupon.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public MemberGroup MemberGroup { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.MemberGroups == null)
            {
                return NotFound();
            }

            var membergroup = await _context.MemberGroups.FirstOrDefaultAsync(m => m.Id == id);

            if (membergroup == null)
            {
                return NotFound();
            }
            else 
            {
                MemberGroup = membergroup;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.MemberGroups == null)
            {
                return NotFound();
            }
            var membergroup = await _context.MemberGroups.FindAsync(id);

            if (membergroup != null)
            {
                MemberGroup = membergroup;
                _context.MemberGroups.Remove(MemberGroup);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
