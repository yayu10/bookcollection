using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookCollectionWebsite.DAL;
using BookCollectionWebsite.Models;
using BookCollectionWebsite.ViewModels;

namespace BookCollectionWebsite.Controllers
{
    public class BooksController : Controller
    {
        private Context db = new Context();

        // GET: Books
        public ActionResult Index(string searchName)
        {
             var books = db.Books.Include(b => b.Category).Include(b => b.Publisher);
            

            if (!String.IsNullOrEmpty(searchName))
            {
               books = db.Books.Where(p => p.Name.Contains(searchName));
            }

            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
