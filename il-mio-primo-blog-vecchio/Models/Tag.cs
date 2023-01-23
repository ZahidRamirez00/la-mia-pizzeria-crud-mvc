

using System.Text.Json.Serialization;

namespace NetCore_01.Models
{
    public class Tag
    {

        public int Id { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        public List<Post>? Post { get; set; }

        public Tag()
        {

        }

    }
}
