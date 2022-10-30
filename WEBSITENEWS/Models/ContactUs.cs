using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEBSITENEWS.Models
{
    public class ContactUs
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [StringLength(15)]
        [DisplayName("اسمك")]
        public string Name { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Email { get; set; }
        public string Subject { get; set; }
    }
}
