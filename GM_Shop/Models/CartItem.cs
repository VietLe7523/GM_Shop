namespace GM_Shop.Models
{
    public class CartItem
    {
        public int MASP { get; set; }
        public string TENSP { get; set; }
        public string ANH { get; set; }
        public decimal DONGIA { get; set; }
        public double DANHGIA { get; set; }
        public int SOLUONG { get; set; }
        public decimal Total => DONGIA * SOLUONG;
    }
}
