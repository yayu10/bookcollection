using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookCollectionWebsite.Models
{
    public class Book
    {

        public Book()
        {
            this.Authors = new HashSet<Author>();
        }

        public int BookID { get; set; }


        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Name { get; set; }

        public string Image { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Edition { get; set; }


        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(400)]
        public string Description { get; set; }

        [Required]
        [DisplayName("Publish Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PublishDate { get; set; }


        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(13)]
        public string ISBN { get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        [Required]
        [DisplayName("Publisher")]
        public int PublisherID { get; set; }

        [ForeignKey("PublisherID")]
        public virtual Publisher Publisher { get; set; }

        [Required]
        [ForeignKey("AuthorID")]
        public virtual ICollection<Author> Authors { get; set; }

        [DisplayName("Ativo?")]
        public bool isActive { get; set; }
    }
}