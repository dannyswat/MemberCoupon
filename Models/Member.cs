using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MemberCoupon.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        [DisplayName("會員號碼")]
        public string Number { get; set; }

        [DisplayName("安全密碼")]
        [Required, StringLength(100)]
        public string SecureKey { get; set; }

        [DisplayName("有效期至")]
        public DateTime? ActiveUntil { get; set; }

        public ICollection<Redemption> Redemptions { get; set; }
    }
}
