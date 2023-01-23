using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Server;
using NetCore_01.Database;
using NetCore_01.Models;

namespace NetCore_01.Utils
{
    public static class TagsConverter
    {

        public static List<SelectListItem> getListTagsForMultipleSelect()
        {
            using (BlogContext db = new BlogContext())
            {
                List<Tag> tagsFromDb = db.Tags.ToList<Tag>();

                // Creare una lista di SelectListItem e tradurci al suo interno tutti i nostri Tag che provengono da Db
                List<SelectListItem> listaPerLaSelectMultipla = new List<SelectListItem>();

                foreach (Tag tag in tagsFromDb)
                {
                    SelectListItem opzioneSingolaSelectMultipla = new SelectListItem() { Text = tag.Title, Value = tag.Id.ToString() };
                    listaPerLaSelectMultipla.Add(opzioneSingolaSelectMultipla);
                }

                return listaPerLaSelectMultipla;
            }
        }
    }
}
