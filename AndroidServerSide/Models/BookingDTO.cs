namespace AndroidServerSide.Models
{
    public class BookingDTO
    {
        public int? Id { get; set; }
        public int ServiceProviderId { get; set; }
        public string CustomerEmail { get; set; }
        public string ServiceName { get; set; }
        public double? Price { get; set; }
        public int noOfHours { get; set; } = 1;
        public string CreatedAt { get; set; }
        public string DeliveryAt { get; set; }
        public string Location { get; set; }
        public string? PaymentOption { get; set; }
        public string? Status { get; set; } = "Active";
    }
}
