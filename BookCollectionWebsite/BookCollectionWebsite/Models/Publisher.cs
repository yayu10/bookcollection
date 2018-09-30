using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookCollectionWebsite.Models
{
    public class Publisher
    {
        public int PublisherID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(120)]
        public string Description { get; set; }

        public bool isActive { get; set; }
    }
}