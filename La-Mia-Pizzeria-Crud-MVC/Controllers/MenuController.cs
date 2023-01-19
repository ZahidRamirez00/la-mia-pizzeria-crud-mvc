using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using La_Mia_Pizzeria_Crud_MVC.Models;
using System.Diagnostics;
using La_Mia_Pizzeria_Crud_MVC.DataBase;
using Microsoft.Extensions.Hosting;
using La_Mia_Pizzeria_Crud_MVC.Utils;
using Azure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace La_Mia_Pizzeria_Crud_MVC.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            using(PizzeContext db = new PizzeContext())
            {
                //List<Pizza> listaDellePizze = PizzeData.GetPizze();
                List<Pizza> listaDellePizze = db.Pizze.ToList<Pizza>();
                return View("Index", listaDellePizze);
            }
        }

        public IActionResult Details(int id)
        {
            using(PizzeContext db = new PizzeContext())
            {
                Pizza pizzaTrovato = db.Pizze
                    .Where(SingolaPizzaNelDb => SingolaPizzaNelDb.Id == id)
                    .Include(pizza => pizza.Category)
                    .Include(pizza => pizza.Ingredients)
                    .FirstOrDefault();

                if (pizzaTrovato != null)
                {
                    return View(pizzaTrovato);
                }
                
                return NotFound("La pizza con l'id cercato non esiste!");
            }
            /*List<Pizza> listaDellePizze = PizzeData.GetPizze();

            foreach (Pizza pizza in listaDellePizze)
            {
                if (pizza.Id == id)
                {
                    return View(pizza);
                }
            }

            return NotFound("Il post con l'id cercato non esiste!");*/
        }

        [HttpGet]
        public IActionResult Create()
        {
            using (PizzeContext db = new PizzeContext())
            {
                List<Category> categoriesFromDb = db.Categories.ToList<Category>();

                PizzaCategoriesView modelForView = new PizzaCategoriesView();
                modelForView.Pizza = new Pizza();

                modelForView.Categories = categoriesFromDb;
                modelForView.Ingredients = IngredientsConverter.getListIngredientsForMultipleSelect();

                return View("Create", modelForView);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaCategoriesView formData)
        {
            if (!ModelState.IsValid)
            {
                using (PizzeContext db = new PizzeContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();
                    formData.Categories = categories;


                    formData.Ingredients = IngredientsConverter.getListIngredientsForMultipleSelect();
                }


                return View("Create", formData);
            }

            using (PizzeContext db = new PizzeContext())
            {
                if (formData.IngredientsSelectedFromMultipleSelect != null)
                {
                    formData.Pizza.Ingredients = new List<Ingredienti>();

                    foreach (string ingredienteId in formData.IngredientsSelectedFromMultipleSelect)
                    {
                        int ingredienteIdIntFromSelect = int.Parse(ingredienteId);

                        Ingredienti ingredienti = db.Ingredients.Where(ingredientiDb => ingredientiDb.Id == ingredienteIdIntFromSelect).FirstOrDefault();

                        // todo controllare eventuali altri errori tipo l'id del tag non esiste

                        formData.Pizza.Ingredients.Add(ingredienti);
                    }
                }

                db.Pizze.Add(formData.Pizza);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzeContext db = new PizzeContext())
            {
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

                if (pizzaToUpdate == null)
                {
                    return NotFound("La pizza non è stata trovata");
                }

                List<Category> categories = db.Categories.ToList<Category>();

                PizzaCategoriesView modelForView = new PizzaCategoriesView();
                modelForView.Pizza = pizzaToUpdate;
                modelForView.Categories = categories;


                List<Ingredienti> listIngredientsFromDb = db.Ingredients.ToList<Ingredienti>();

                List<SelectListItem> listaIngredientiPerLaSelect = new List<SelectListItem>();

                foreach (Ingredienti ingrediente in listIngredientsFromDb)
                {
                    // Ricerco se il tag che sto inserindo nella lista delle opzioni della select era già stato selezionato dall'utente
                    // all'interno della lista dei tag del post da modificare
                    bool eraStatoSelezionato = pizzaToUpdate.Ingredients.Any(ingredientiSelezionati => ingredientiSelezionati.Id == ingrediente.Id);

                    SelectListItem ingredienteSingoloSelect = new SelectListItem() { Text = ingrediente.Ingrediente, Value = ingrediente.Id.ToString(), Selected = eraStatoSelezionato };
                    listaIngredientiPerLaSelect.Add(ingredienteSingoloSelect);
                }

                modelForView.Ingredients = listaIngredientiPerLaSelect;

                return View("Update", pizzaToUpdate);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id,PizzaCategoriesView formData)
        {
            if (!ModelState.IsValid)
            {
                using (PizzeContext db = new PizzeContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();

                    formData.Categories = categories;
                }

                return View("Update", formData);
            }

            using (PizzeContext db = new PizzeContext())
            {
                Pizza pizzaToUpdate = db.Pizze.Where(ingrediente => ingrediente.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

                if (pizzaToUpdate != null)
                {
                    pizzaToUpdate.Name = formData.Pizza.Name;
                    pizzaToUpdate.Description = formData.Pizza.Description;
                    pizzaToUpdate.CategoryId = formData.Pizza.CategoryId;
                    pizzaToUpdate.Image = formData.Pizza.Image;
                    pizzaToUpdate.Prezzo = formData.Pizza.Prezzo;

                    pizzaToUpdate.Ingredients.Clear();

                    if (formData.IngredientsSelectedFromMultipleSelect != null)
                    {

                        foreach (string ingrediId in formData.IngredientsSelectedFromMultipleSelect)
                        {
                            int ingredienteIdIntFromSelect = int.Parse(ingrediId);

                            Ingredienti ingredinte = db.Ingredients.Where(ingredinteDb => ingredinteDb.Id == ingredienteIdIntFromSelect).FirstOrDefault();

                            // todo controllare eventuali altri errori tipo l'id del tag non esiste

                            pizzaToUpdate.Ingredients.Add(ingredinte);
                        }
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La pizza che volevi modificare non è stata trovata!");
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using (PizzeContext db = new PizzeContext())
            {
                Pizza pizzaToDelete = db.Pizze.Where(Pizza => Pizza.Id == id).FirstOrDefault();

                if (pizzaToDelete != null)
                {
                    db.Pizze.Remove(pizzaToDelete);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La pizza da eliminare non è stata trovata!");
                }
            }
        }

        [HttpDelete]
        public IActionResult ProvaDelete()
        {
            return View("Create");
        }
    }
}
