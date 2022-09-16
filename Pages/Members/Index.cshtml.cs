using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MemberCoupon.Data;
using MemberCoupon.Models;
using MemberCoupon.Common;

namespace MemberCoupon.Pages.Members
{
    public class IndexModel : PageModel
    {
        private readonly MemberCoupon.Data.ApplicationDbContext _context;
        private readonly MemberService service;

        public IndexModel(MemberCoupon.Data.ApplicationDbContext context, MemberService service)
        {
            _context = context;
            this.service = service;
        }

        public PagerViewModel<Member> ItemList { get;set; } = new PagerViewModel<Member>();

        public async Task OnGetAsync([FromQuery] ListItemsModel listModel)
        {
            if (_context.Members != null)
            {
                ItemList.CurrentPage = listModel.Page;
                ItemList.PageSize = listModel.PageSize;
                ItemList.Items = await service.ListAsync(listModel);
                ItemList.TotalRecordsCount = await service.CountAsync(listModel.Filters);
            }
        }
    }
}
