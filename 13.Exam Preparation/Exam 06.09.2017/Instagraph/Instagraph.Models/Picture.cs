namespace Instagraph.Models
{
    using System.Collections.Generic;

    public class Picture
    {
        public Picture()
        {
            this.Users = new List<User>();
            this.Posts = new List<Post>();
        }

        public int Id { get; set; }

        public string Path { get; set; }

        public decimal Size { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
