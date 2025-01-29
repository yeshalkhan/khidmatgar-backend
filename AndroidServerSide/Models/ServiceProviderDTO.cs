namespace AndroidServerSide.Models
{
    public class ServiceProviderDTO
    {
        public int? Id { get; set; }
        public string ServiceProviderName { get; set; }
        public string? Image {get; set;}
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
        public int? Discount { get; set; }
        public float? Rating { get; set; }
        public string ServiceName { get; set; }
    }
}
