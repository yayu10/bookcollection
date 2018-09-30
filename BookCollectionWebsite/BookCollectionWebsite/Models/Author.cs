using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookCollectionWebsite.Models
{
    public class Author
    {
        public Author()
        {
            this.PublishedBooks = new HashSet<Book>();
        }


        public int AuthorID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(80)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(80)]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(400)]
        public string Bio { get; set; }

        [ForeignKey("BookID")]
        public virtual ICollection<Book> PublishedBooks { get; set; }

        public bool isActive { get; set; }
    }
}