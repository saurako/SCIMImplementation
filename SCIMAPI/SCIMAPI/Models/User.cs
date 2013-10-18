using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCIMAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid user name")]
        public string userName { get; set; }

        [Required]
        public string displayName { get; set; }

        public Boolean active { get; set; }    
               
    }
}