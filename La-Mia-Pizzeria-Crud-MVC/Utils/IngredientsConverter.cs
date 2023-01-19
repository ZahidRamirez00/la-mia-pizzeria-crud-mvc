using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Server;
using La_Mia_Pizzeria_Crud_MVC.DataBase;
using La_Mia_Pizzeria_Crud_MVC.Models;

namespace La_Mia_Pizzeria_Crud_MVC.Utils
{
    public class IngredientsConverter
    {
        public static List<SelectListItem> getListIngredientsForMultipleSelect()
        {
            using (PizzeContext db = new PizzeContext())
            {
                List<Ingredienti> ingredientsFromDb = db.Ingredients.ToList<Ingredienti>();

                // Creare una lista di SelectListItem e tradurci al suo interno tutti i nostri Tag che provengono da Db
                List<SelectListItem> listaPerLaSelectMultipla = new List<SelectListItem>();

                foreach (Ingredienti ingrediente in ingredientsFromDb)
                {
                    SelectListItem opzioneSingolaSelectMultipla = new SelectListItem() { Text = ingrediente.Ingrediente, Value = ingrediente.Id.ToString() };
                    listaPerLaSelectMultipla.Add(opzioneSingolaSelectMultipla);
                }

                return listaPerLaSelectMultipla;
            }
        }
    }
}
