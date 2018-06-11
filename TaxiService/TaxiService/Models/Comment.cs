using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.Models
{
    public class Comment
    {
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public User CreatedBy { get; set; }
        public Drive CommentedOn { get; set; }
        public int Grade { get; set; }
    }
}