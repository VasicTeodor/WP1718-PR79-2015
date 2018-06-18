using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.Models
{
    public class LoginDto
    {
        public string AccessToken { get; set; }
        public User User { get; set; }
    }
}