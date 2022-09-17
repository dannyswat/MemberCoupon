using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MemberCoupon.Models
{
    public class MemberGroup
    {
        public int Id { get; set; }

        [DisplayName("名稱")]
        [Required, StringLength(100)]
        public string Name { get; set; }

        [DisplayName("失效")]
        public bool IsDisabled { get; set; }
    }
}
