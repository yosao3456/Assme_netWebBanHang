namespace Assm_KiemThuNangCao.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("carts")]
    public class Cart
    {
        public int CartID { get; set; }
        public int? CustomerID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Customer? Customer { get; set; }


        public ICollection<CartDetail>? CartDetails { get; set; }
    }
}
