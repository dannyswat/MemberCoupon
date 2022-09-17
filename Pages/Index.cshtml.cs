using MemberCoupon.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MemberCoupon.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public void OnGet()
        {

        }

        public IActionResult OnGetRedemptionCount()
        {
            return new JsonResult(new { count = context.Redemptions.Where(r => r.Date.Date == Constants.CurrentTime().Date).Count() });
        }
    }
}