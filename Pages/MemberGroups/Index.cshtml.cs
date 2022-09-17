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
    public class IndexModel : PageModel
    {
        private readonly MemberCoupon.Data.ApplicationDbContext _context;

        public IndexModel(MemberCoupon.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<MemberGroup> MemberGroup { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.MemberGroups != null)
            {
                MemberGroup = await _context.MemberGroups.ToListAsync();
            }
        }
    }
}
