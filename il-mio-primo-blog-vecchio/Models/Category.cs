namespace NetCore_01.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<Post> Posts { get; set; }

        public Category()
        {

        }
    }
}
