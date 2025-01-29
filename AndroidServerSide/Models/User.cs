using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AndroidServerSide.Models
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Location { get; set; }
        public string PhoneNo { get; set; }
    }

}
