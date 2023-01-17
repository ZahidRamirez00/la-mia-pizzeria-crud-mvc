namespace La_Mia_Pizzeria_Crud_MVC.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<Pizza> Posts { get; set; }

        public Category()
        {

        }
    }
}
