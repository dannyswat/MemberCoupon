using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MemberCoupon.Models
{
    public class Organization
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        [DisplayName("機構名稱")]
        public string Name { get; set; }

        [StringLength(4000)]
        [DisplayName("頁首")]
        public string PageHeader { get; set; }


    }
}
