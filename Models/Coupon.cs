using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MemberCoupon.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        [DisplayName("換領券名稱")]
        public string Name { get; set; }

        [Required, StringLength(4000)]
        [DisplayName("換領券描述")]
        public string Description { get; set; }

        [DisplayName("開始顯示時間")]
        public DateTime ShowStart { get; set; }

        [DisplayName("結束顯示時間")]
        public DateTime ShowEnd { get; set; }

        [DisplayName("只限會員組別")]
        public int? ExclusiveMemberGroupId { get; set; }

        public MemberGroup ExclusiveMemberGroup { get; set; }

        [DisplayName("取消")]
        public bool Cancelled { get; set; }

        [StringLength(4000)]
        [DisplayName("取消原因")]
        public string CancelReason { get; set; }

        public ICollection<Redemption> Redemptions { get; set; }
    }
}
