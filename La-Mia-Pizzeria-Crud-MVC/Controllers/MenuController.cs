using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using La_Mia_Pizzeria_1.Models;
using La_Mia_Pizzeria_1.Utils;
using System.Diagnostics;
using La_Mia_Pizzeria_1.DataBase;
using Microsoft.Extensions.Hosting;

namespace La_Mia_Pizzeria_1.Controllers
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
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", formData);
            }

            using (PizzeContext db = new PizzeContext())
            {
                db.Pizze.Add(formData);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzeContext db = new PizzeContext())
            {
                Pizza pizzaToUpdate = db.Pizze.Where(Pizza => Pizza.Id == id).FirstOrDefault();

                if (pizzaToUpdate == null)
                {
                    return NotFound("La pizza non è stata trovata");
                }

                return View("Update", pizzaToUpdate);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Pizza formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", formData);
            }

            using (PizzeContext db = new PizzeContext())
            {
                Pizza pizzaToUpdate = db.Pizze.Where(Pizza => Pizza.Id == formData.Id).FirstOrDefault();

                if (pizzaToUpdate != null)
                {
                    pizzaToUpdate.Name = formData.Name;
                    pizzaToUpdate.Description = formData.Description;
                    pizzaToUpdate.Image = formData.Image;
                    pizzaToUpdate.Prezzo = formData.Prezzo;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La pizza che volevi modificare non è stata trovata!");
                }
            }

        }

        
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
    }
}
