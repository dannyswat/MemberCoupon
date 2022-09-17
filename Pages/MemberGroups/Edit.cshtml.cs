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

namespace MemberCoupon.Pages.MemberGroups
{
    public class EditModel : PageModel
    {
        private readonly MemberCoupon.Data.ApplicationDbContext _context;

        public EditModel(MemberCoupon.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MemberGroup MemberGroup { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.MemberGroups == null)
            {
                return NotFound();
            }

            var membergroup =  await _context.MemberGroups.FirstOrDefaultAsync(m => m.Id == id);
            if (membergroup == null)
            {
                return NotFound();
            }
            MemberGroup = membergroup;
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

            _context.Attach(MemberGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberGroupExists(MemberGroup.Id))
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

        private bool MemberGroupExists(int id)
        {
          return _context.MemberGroups.Any(e => e.Id == id);
        }
    }
}
