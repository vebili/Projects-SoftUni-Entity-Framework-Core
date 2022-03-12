using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

using Instagraph.Data;
using Instagraph.Models;
using System.IO;
using Instagraph.Models.Dto;
using System.Xml.Serialization;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            Picture[] pictures = JsonConvert.DeserializeObject<Picture[]>(jsonString);
            List<string> paths = new List<string>();

            var sb = new StringBuilder();

            foreach (var p in pictures)
            {
                if(paths.Any(pa => pa == p.Path) || p.Path == string.Empty || p.Path == null || p.Size <= 0)
                {
                    sb.AppendLine("Error: Invalid data.");
                }
                else
                {
                    paths.Add(p.Path);
                    context.Pictures.Add(p);
                    context.SaveChanges();
                    sb.AppendLine($"Successfully imported Picture {p.Path}.");
                }
            }

            return sb.ToString().Trim();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            UserDto[] users = JsonConvert.DeserializeObject<UserDto[]>(jsonString);
             
            string[] picturePaths = context.Pictures
                .Select(p => p.Path)
                .ToArray();

            List<string> usernames = new List<string>();

            var sb = new StringBuilder();

            foreach (var user in users)
            {
                if(user.Username == null || user.Password == null || user.ProfilePicture == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                }
                else if(!(picturePaths.Any(p => p == user.ProfilePicture)) || 
                    user.Username.Length > 30 ||
                    usernames.Any(u => u == user.Username) ||
                    user.Password.Length > 20)
                {
                    sb.AppendLine("Error: Invalid data.");
                }
                else
                {
                    usernames.Add(user.Username);

                    User userDb = new User
                    {
                        Username = user.Username,
                        Password = user.Password,
                        ProfilePicture = context.Pictures.Single(p => p.Path == user.ProfilePicture)
                    };

                    context.Users.Add(userDb);
                    context.SaveChanges();

                    sb.AppendLine($"Successfully imported User {user.Username}.");
                }
            }

            return sb.ToString().Trim();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var usersFollowers = JsonConvert.DeserializeObject<UserFollowerDto[]>(jsonString);

            var sb = new StringBuilder();

            foreach (var uf in usersFollowers)
            {
                if (uf.Follower == null || uf.User == null ||
                !context.Users.Any(u => u.Username == uf.User) || !context.Users.Any(u => u.Username == uf.Follower) || context.UsersFollowers.Any(u => u.User.Username == uf.User && u.Follower.Username == uf.Follower))
                {
                    sb.AppendLine("Error: Invalid data.");
                }
                else
                {
                    var userFollower = new UserFollower()
                    {
                        User = context.Users.Single(u => u.Username == uf.User),
                        Follower = context.Users.Single(u => u.Username == uf.Follower)
                    };

                    context.UsersFollowers.Add(userFollower);
                    context.SaveChanges();

                    sb.AppendLine($"Successfully imported Follower {uf.Follower} to User {uf.User}.");
                }
            }           

            return sb.ToString().Trim();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var xDoc = XDocument.Parse(xmlString);

            var posts = xDoc.Root.Elements();

            var sb = new StringBuilder();

            foreach (var post in posts)
            {
                var caption = post.Element("caption")?.Value;
                var user = post.Element("user")?.Value;
                var picture = post.Element("picture")?.Value;

                if(user == null || picture == null)
                {
                    sb.AppendLine("Error: Invalid data.");
                }
                else if(!context.Pictures.Any(p => p.Path == picture) || 
                    !context.Users.Any(u => u.Username == user))
                {
                    sb.AppendLine("Error: Invalid data.");
                }
                else
                {
                    var postDb = new Post()
                    {
                        User = context.Users.Single(u => u.Username == user),
                        Picture = context.Pictures.Single(p => p.Path == picture)
                    };

                    if(caption != null)
                    {
                        postDb.Caption = caption;
                    }

                    context.Posts.Add(postDb);
                    context.SaveChanges();

                    sb.AppendLine($"Successfully imported Post {caption}.");
                }
            }
            return sb.ToString().Trim();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var xDoc = XDocument.Parse(xmlString);

            var comments = xDoc.Root.Elements();

            var sb = new StringBuilder();

            foreach (var comment in comments)
            {
                var content = comment.Element("content")?.Value;
                var user = comment.Element("user")?.Value;
                var post = comment.Element("post")?.Attribute("id")?.Value;

                int postId;
                if(user == null || !int.TryParse(post, out postId))
                {
                    sb.AppendLine("Error: Invalid data.");
                }
                else if (!context.Users.Any(u => u.Username == user) || !context.Posts.Any(p => p.Id == postId))
                {
                    sb.AppendLine("Error: Invalid data.");
                }
                else
                {
                    var newComment = new Comment()
                    {
                        User = context.Users.Single(u => u.Username == user),
                        Post = context.Posts.Find(postId),
                        Content = content
                    };

                    context.Comments.Add(newComment);
                    context.SaveChanges();

                    sb.AppendLine($"Successfully imported Comment {content}.");
                }                
            }

            return sb.ToString().Trim();
        }
    }
}
