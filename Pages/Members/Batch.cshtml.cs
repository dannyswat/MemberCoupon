using MemberCoupon.Data;
using MemberCoupon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace MemberCoupon.Pages.Members
{
    public class BatchModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public BatchModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        [BindProperty]
        [DisplayName("會員編號")]
        [Required]
        public string Numbers { get; set; }

        [BindProperty]
        [DisplayName("動作")]
        [Required]
        public string Action { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string[] memberNos = Numbers.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            switch (this.Action)
            {
                case "Add":

                    foreach (var memberNo in memberNos)
                    {
                        var member = new Member
                        {
                            Number = memberNo
                        };

                        byte[] keyBytes = RandomNumberGenerator.GetBytes(8);
                        member.SecureKey = BitConverter.ToString(keyBytes).Replace("-", "").ToLower();

                        context.Members.Add(member);
                    }

                    await context.SaveChangesAsync();

                    break;
                case "Print":
                    return Redirect("/Members/Print?members=" + string.Join(",", memberNos));

                case "Disable":

                    foreach (var memberNo in memberNos)
                    {
                        if (string.IsNullOrEmpty(memberNo)) continue;

                        var member = context.Members.FirstOrDefault(e => e.Number == memberNo);

                        if (member == null)
                            throw new InvalidOperationException($"Member does not exist {memberNo}");

                        member.ActiveUntil = Constants.CurrentTime().AddMinutes(-1);
                    }

                    await context.SaveChangesAsync();

                    break;
                default:
                    throw new NotSupportedException($"Action not supported {Action}");
            }

            return RedirectToPage("./Index");
        }
    }
}
