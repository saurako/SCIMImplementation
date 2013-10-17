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

        [DataType(DataType.EmailAddress)]
        public string userName { get; set; }

        public string displayName { get; set; }

        public Boolean active { get; set; }    
               
    }
}