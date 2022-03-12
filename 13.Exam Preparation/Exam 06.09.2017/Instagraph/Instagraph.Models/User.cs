namespace Instagraph.Models
{
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.Followers = new List<UserFollower>();
            this.UsersFollowing = new List<UserFollower>();
            this.Posts = new List<Post>();
            this.Comments = new List<Comment>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int ProfilePictureId { get; set; }
        public Picture ProfilePicture { get; set; }

        public ICollection<UserFollower> Followers { get; set; }

        public ICollection<UserFollower> UsersFollowing { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
