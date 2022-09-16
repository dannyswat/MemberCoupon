namespace MemberCoupon.Models
{
    public class Redemption
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int CouponId { get; set; }

        public DateTime Date { get; set; }

        public Coupon Coupon { get; set; }

        public Member Member { get; set; }
    }
}
