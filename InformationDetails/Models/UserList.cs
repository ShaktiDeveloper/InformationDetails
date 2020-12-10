using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationDetails.Models
{
    public class UserList
    {
        public int Id { get; set; }
        [Required]
        [Remote("IsUserExists", "List", HttpMethod = "POST", ErrorMessage = "Email already exists")]
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CustomerAddress { get; set; }
        public string City { get; set; }
        public string images { get; set; }
        public string Role { get; set; }
    }
}