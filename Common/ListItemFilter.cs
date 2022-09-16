namespace MemberCoupon.Common
{
    public class ListItemsFilter
    {
        public string Property { get; set; }

        public string Operator { get; set; } = "=";

        public object Value { get; set; }
    }
}
