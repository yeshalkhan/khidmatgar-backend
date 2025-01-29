using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AndroidServerSide.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int ServiceProviderId { get; set; }
        public string CustomerEmail { get; set; }
        public string ServiceName { get; set; }
        public double Price { get; set; }
        public int noOfHours { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime DeliveryAt { get; set; }
        public string Location { get; set; }
        public string PaymentOption { get; set; }
        public string Status { get; set; } = "Active";
    }
}
