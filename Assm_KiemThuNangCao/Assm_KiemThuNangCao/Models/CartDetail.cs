namespace Assm_KiemThuNangCao.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

[Table("cart_details")]
    public class CartDetail
    {
        public int CartDetailID { get; set; }
        public int? CartID { get; set; }
        public int? ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Cart? Cart { get; set; }
        public Product? Product { get; set; }
    }
}
