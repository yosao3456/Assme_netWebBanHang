namespace Assm_KiemThuNangCao.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public int? CategoryID { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }

        public Category? Category { get; set; }
    }
}
