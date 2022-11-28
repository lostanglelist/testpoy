namespace TestKuy.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public ImageModel image { get; set; }
        public Shipping shipping { get; set; }
        public float Quantity { get; set; }
        public string CartId { get; set; }
    }
}
