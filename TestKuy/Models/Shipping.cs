using System.ComponentModel.DataAnnotations;

namespace TestKuy.Models
{
    public class Shipping
    {
        [Key]
        public string ShipName { get; set; }
        public int ShipPhone { get; set; }
        public string ShipCountry { get; set; }
        public string ShipState { get; set; }
        public int ShipZipcode { get; set; }
    }
}
