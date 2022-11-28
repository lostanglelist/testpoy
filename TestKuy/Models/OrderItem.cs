using System.ComponentModel.DataAnnotations.Schema;

namespace TestKuy.Models
{
    public class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public string OrderId { get; set; }
        public string VGAId { get; set; }
        public string VGAimage { get; set; }
        public string VGAName { get; set; }
        public string UserName { get; set; }
        public int Phone { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public int Zipcode { get; set; }
        public Order order { get; set; }
        public ImageModel image { get; set; }
        public Shipping shipping { get; set; }
    }
}
