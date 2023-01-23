using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore_01.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        [StringLength(100, ErrorMessage="Il campo titolo non può contenere più di 100 caratteri")]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [Column(TypeName = "varchar(300)")]
        [StringLength(300, ErrorMessage = "Il campo titolo non può contenere più di 100 caratteri")]
        public string Image { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }


        public List<Tag>? Tags { get; set; }

        public Post()
        {

        }

        public Post(string title, string description, string image)
        {
            Title = title;
            Description = description;
            Image = image;
        }

    }
}
