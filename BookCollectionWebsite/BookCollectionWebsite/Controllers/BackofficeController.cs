using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookCollectionWebsite.DAL;
using BookCollectionWebsite.Models;
using BookCollectionWebsite.ViewModels;

namespace BookCollectionWebsite.Controllers
{
    public class BackofficeController : Controller
    {
        private Context db = new Context();

        // GET: Books
        [Authorize]
        public ActionResult Books(string searchName)
        {
            var books = db.Books.Include(b => b.Category).Include(b => b.Publisher);


            if (!String.IsNullOrEmpty(searchName))
            {
                books = db.Books.Where(p => p.Name.Contains(searchName));
            }

            return View(books.ToList());
        }

        // GET: Books/Details/5
        [Authorize]
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

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {
            var book = new Book();
            book.Authors = new List<Author>();
            PopulateAssignedAuthor(book);

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description");
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Description");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "BookID,Name,Image,Edition,Description,PublishDate,ISBN,CategoryID,PublisherID,isActive")] Book book, string[] selectedAuthors, HttpPostedFileBase file)
        {

             if (file != null)
             {
                string imageName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                string extension = System.IO.Path.GetExtension(file.FileName);
                imageName = imageName + DateTime.Now.ToString("yyyyMMdd") + extension;
                imageName = System.IO.Path.Combine(Server.MapPath("~/Images/"), imageName);
                file.SaveAs(imageName);
                book.Image = imageName;
            } 

            if (selectedAuthors != null)
            {
                book.Authors = new List<Author>();
                foreach (var author in selectedAuthors)
                {
                    var authorBeingAdded = db.Authors.Find(int.Parse(author));
                    book.Authors.Add(authorBeingAdded);
                }
            }

            if (ModelState.IsValid)
            {

                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Books");
            }


            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description", book.CategoryID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Description", book.PublisherID);
            PopulateAssignedAuthor(book);

            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Include(p => p.Authors).Where(i => i.BookID == id).Single();
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description", book.CategoryID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Description", book.PublisherID);
            PopulateAssignedAuthor(book);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int? id, string[] selectedAuthors)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bookToUpdate = db.Books.Include(p => p.Authors).Where(i => i.BookID == id).Single();

            if (TryUpdateModel(bookToUpdate, "", new string[] { "BookID", "Name", "Image", "Edition", "Description", "PublishDate", "ISBN", "CategoryID", "PublisherID", "isActive" }))
            {
                try
                {
                    UpdateBookAuthors(selectedAuthors, bookToUpdate);

                    db.Entry(bookToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Books");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }



            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Description", bookToUpdate.CategoryID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Description", bookToUpdate.PublisherID);
            PopulateAssignedAuthor(bookToUpdate);
            return View(bookToUpdate);
        }

        // GET: Books/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Books");
        }

       
        private void PopulateAssignedAuthor(Book book)
        {
            var allAuthors = db.Authors;
            var bookAuthors = new HashSet<int>(book.Authors.Select(b => b.AuthorID));
            var viewModel = new List<BookAuthorVM>();
            foreach (var author in allAuthors)
            {
                viewModel.Add(
                 new BookAuthorVM
                 {

                     AuthorID = author.AuthorID,
                     AuthorName = author.Name + " " + author.LastName,
                     Assigned = bookAuthors.Contains(author.AuthorID)
                 }
                    );
            }
            ViewBag.Authors = viewModel;

        }

        private void UpdateBookAuthors(string[] selectedAuthors, Book bookToUpdate)
        {
            if (selectedAuthors == null)
            {
                bookToUpdate.Authors = new List<Author>();
                return;
            }

            var selectedAuthorsHash = new HashSet<string>(selectedAuthors);
            var bookAuthors = new HashSet<int>(bookToUpdate.Authors.Select(b => b.AuthorID));

            foreach (var author in db.Authors)
            {
                if (selectedAuthorsHash.Contains(author.AuthorID.ToString()))
                {
                    if (!bookAuthors.Contains(author.AuthorID))
                    {
                        bookToUpdate.Authors.Add(author);
                    }
                }
                else
                {
                    if (bookAuthors.Contains(author.AuthorID))
                    {
                        bookToUpdate.Authors.Remove(author);
                    }
                }
            }
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
