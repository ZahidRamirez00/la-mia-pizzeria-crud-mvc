using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCore_01.Database;
using NetCore_01.Models;
using NetCore_01.Utils;
using System.Diagnostics;

namespace NetCore_01.Controllers
{
    public class PostController : Controller
    {
 
        public IActionResult Index()
        {
            using(BlogContext db = new BlogContext())
            {
                List<Post> listaDeiPost = db.Posts.ToList<Post>();
                return View("Index", listaDeiPost);
            }

        }

        public IActionResult Details(int id)
        {
            using (BlogContext db = new BlogContext())
            {
                // LINQ: syntax methos
                Post postTrovato = db.Posts
                    .Where(SingoloPostNelDb => SingoloPostNelDb.Id == id)
                    .Include(post=>post.Category)
                    .Include(post => post.Tags)
                    .FirstOrDefault();

                if (postTrovato != null)
                {
                    return View(postTrovato);
                }

                return NotFound("Il post con l'id cercato non esiste!");

            }

        }


        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            using(BlogContext db = new BlogContext())
            {
                List<Category> categoriesFromDb = db.Categories.ToList<Category>();
               
                PostCategoriesView modelForView = new PostCategoriesView();
                modelForView.Post = new Post();

                modelForView.Categories = categoriesFromDb;
                modelForView.Tags = TagsConverter.getListTagsForMultipleSelect();

                return View("Create", modelForView);
            }

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(PostCategoriesView formData)
        {
            if (!ModelState.IsValid)
            {
                using(BlogContext db = new BlogContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();
                    formData.Categories = categories;

                   
                    formData.Tags = TagsConverter.getListTagsForMultipleSelect();
                }
                

                return View("Create", formData);
            }

            using(BlogContext db = new BlogContext())
            {
                if(formData.TagsSelectedFromMultipleSelect != null)
                {
                    formData.Post.Tags = new List<Tag>();

                    foreach (string tagId in formData.TagsSelectedFromMultipleSelect)
                    {
                        int tagIdIntFromSelect = int.Parse(tagId);

                        Tag tag = db.Tags.Where(tagDb => tagDb.Id == tagIdIntFromSelect).FirstOrDefault();

                        // todo controllare eventuali altri errori tipo l'id del tag non esiste

                        formData.Post.Tags.Add(tag);
                    }
                }

                db.Posts.Add(formData.Post);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize]
        public IActionResult Update(int id)
        {
            using(BlogContext db = new BlogContext())
            {
                Post postToUpdate = db.Posts.Where(articolo => articolo.Id == id).Include(post=>post.Tags).FirstOrDefault();

                if(postToUpdate == null)
                {
                    return NotFound("Il post non è stato trovato");
                }

                List<Category> categories = db.Categories.ToList<Category>();

                PostCategoriesView modelForView = new PostCategoriesView();
                modelForView.Post = postToUpdate;
                modelForView.Categories = categories;
                

                List<Tag> listTagFromDb = db.Tags.ToList<Tag>();

                List<SelectListItem> listaOpzioniPerLaSelect = new List<SelectListItem>();

                foreach (Tag tag in listTagFromDb)
                {
                    // Ricerco se il tag che sto inserindo nella lista delle opzioni della select era già stato selezionato dall'utente
                    // all'interno della lista dei tag del post da modificare
                    bool eraStatoSelezionato = postToUpdate.Tags.Any(tagSelezionati => tagSelezionati.Id == tag.Id);

                    SelectListItem opzioneSingolaSelect = new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString(), Selected = eraStatoSelezionato };
                    listaOpzioniPerLaSelect.Add(opzioneSingolaSelect);
                }

                modelForView.Tags = listaOpzioniPerLaSelect;

                return View("Update", modelForView);
            }
  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Update(int id, PostCategoriesView formData)
        {
            if (!ModelState.IsValid)
            {

                using (BlogContext db = new BlogContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();

                    formData.Categories = categories;
                }

                return View("Update", formData);
            }

            using (BlogContext db = new BlogContext())
            {
                Post postToUpdate = db.Posts.Where(articolo => articolo.Id == id).Include(post=>post.Tags).FirstOrDefault();

                if (postToUpdate != null)
                {
            
                    postToUpdate.Title = formData.Post.Title;
                    postToUpdate.Description = formData.Post.Description;
                    postToUpdate.Image = formData.Post.Image;
                    postToUpdate.CategoryId = formData.Post.CategoryId;

                    // rimuoviamo i tag e inseriamo i nuovi
                    postToUpdate.Tags.Clear();

                    if (formData.TagsSelectedFromMultipleSelect != null)
                    {
  
                        foreach (string tagId in formData.TagsSelectedFromMultipleSelect)
                        {
                            int tagIdIntFromSelect = int.Parse(tagId);

                            Tag tag = db.Tags.Where(tagDb => tagDb.Id == tagIdIntFromSelect).FirstOrDefault();

                            // todo controllare eventuali altri errori tipo l'id del tag non esiste

                            postToUpdate.Tags.Add(tag);
                        }
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Il post che volevi modificare non è stato trovato!");
                }
            }
   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Delete(int id)
        {
            using(BlogContext db = new BlogContext())
            {
                Post postToDelete = db.Posts.Where(post => post.Id == id).FirstOrDefault();

                if(postToDelete != null)
                {
                    db.Posts.Remove(postToDelete);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                } else
                {
                    return NotFound("Il post da eliminare non è stato trovato!");
                }
            }
        }

    }
}