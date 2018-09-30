using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCollectionWebsite.ViewModels
{
    public class BookAuthorVM
    {
        public int AuthorID { get; set; }

        public string AuthorName { get; set; }

        public bool Assigned { get; set; }
    }
}