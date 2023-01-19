namespace La_Mia_Pizzeria_Crud_MVC.Models
{
    public class Ingredienti
    {
        public int Id { get; set; }
        public string Ingrediente { get; set; }

        public List<Pizza> Pizza { get; set; }

        public Ingredienti()
        {

        }
    }
}
