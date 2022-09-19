using MemberCoupon.Common;
using MemberCoupon.Data;
using MemberCoupon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;

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
        public string Numbers { get; set; }

        [BindProperty]
        [DisplayName("動作")]
        [Required]
        public string Action { get; set; }

        [BindProperty]
        [DisplayName("會員組別")]
        public int? MemberGroupId { get; set; }

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

            string[] memberNos = (Numbers ?? "").Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            switch (this.Action)
            {
                case "Add":
                    if (memberNos.Length == 0)
                    {
                        ModelState.AddModelError(nameof(Numbers), "請輸入會員編號");
                        return Page();
                    }

                    foreach (var memberNo in memberNos)
                    {
                        var member = new Member
                        {
                            Number = memberNo,
                            MemberGroupId = MemberGroupId
                        };

                        byte[] keyBytes = RandomNumberGenerator.GetBytes(8);
                        member.SecureKey = BitConverter.ToString(keyBytes).Replace("-", "").ToLower();

                        context.Members.Add(member);
                    }

                    await context.SaveChangesAsync();

                    break;
                case "Print":
                    if (memberNos.Length > 0)
                        return Redirect("/Members/Print?members=" + WebUtility.UrlEncode(string.Join(",", memberNos)));
                    else if (MemberGroupId.HasValue)
                        return Redirect("/Members/Print?Filters=" + JsonSerializer.Serialize(new ListItemsFilter[] { new ListItemsFilter { Property = nameof(Member.MemberGroupId), Value = MemberGroupId.Value } }));
                    else
                        return RedirectToPage("./Print");

                case "Disable":
                case "Reset":
                    if (memberNos.Length == 0 && !MemberGroupId.HasValue)
                    {
                        ModelState.AddModelError(nameof(Numbers), "請輸入會員編號或選擇會員組別");
                        return Page();
                    }

                    var now = Constants.CurrentTime();

                    foreach (var memberNo in memberNos)
                    {
                        if (string.IsNullOrEmpty(memberNo)) continue;

                        var member = context.Members.FirstOrDefault(e => e.Number == memberNo);

                        if (member == null)
                            throw new InvalidOperationException($"Member does not exist {memberNo}");

                        if (Action == "Disable")
                            member.ActiveUntil = now.AddMinutes(-1);
                        else
                        {
                            byte[] keyBytes = RandomNumberGenerator.GetBytes(8);
                            member.SecureKey = BitConverter.ToString(keyBytes).Replace("-", "").ToLower();
                        }
                    }

                    if (MemberGroupId.HasValue)
                    {
                        var members = context.Members.Where(e =>
                            e.MemberGroupId == MemberGroupId.Value &&
                            (!e.ActiveUntil.HasValue || e.ActiveUntil.Value > now)).ToList();
                        foreach (var member in members)
                            if (Action == "Disable")
                                member.ActiveUntil = now.AddMinutes(-1);
                            else
                            {
                                byte[] keyBytes = RandomNumberGenerator.GetBytes(8);
                                member.SecureKey = BitConverter.ToString(keyBytes).Replace("-", "").ToLower();
                            }
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
