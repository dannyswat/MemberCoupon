using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MemberCoupon.Data;
using MemberCoupon.Models;
using System.Security.Cryptography;

namespace MemberCoupon.Pages.Members
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
        public Member Member { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Member.SecureKey == "auto")
            {
                byte[] keyBytes = RandomNumberGenerator.GetBytes(8);
                Member.SecureKey = BitConverter.ToString(keyBytes).Replace("-", "").ToLower();
            }

            _context.Members.Add(Member);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
