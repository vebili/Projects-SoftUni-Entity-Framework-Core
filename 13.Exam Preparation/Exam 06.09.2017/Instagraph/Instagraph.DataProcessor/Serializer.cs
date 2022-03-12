using System;

using Instagraph.Data;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {

            var posts = context.Posts
                .Where(p => p.Comments.Count() == 0)
                .OrderBy(p => p.Id)
                .Select(p => new
                {
                    Id = p.Id,
                    Picture = p.Picture.Path,
                    User = p.User.Username
                })
                .ToArray();

            var json = JsonConvert.SerializeObject(posts, Formatting.Indented);
            
            return json;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            var users = context.Users
                .Where(u => u.Posts
                    .Any(p => p.Comments
                        .Any(c => u.Followers
                            .Any(f => f.FollowerId == c.UserId))))
                .OrderBy(u => u.Id)
                .Select(u => new
                {
                    Username = u.Username,
                    Followers = u.Followers.Count()
                })
                .ToArray();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            
            return json;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users = context.Users
                .Include(u => u.Posts)
                .ThenInclude(p => p.Comments)
                .Select(u => new
                {
                    Username = u.Username,
                    MostComments = (u.Posts.Count() == 0 ? 0 : u.Posts.Max(p => p.Comments.Count()))
                })
                .OrderByDescending(u => u.MostComments)
                .ThenBy(u => u.Username)
                .ToArray();

            var xDoc = new XDocument(new XElement("users"));

            foreach (var user in users)
            {
                var userElement = new XElement("user");

                userElement.Add(new XElement("Username", user.Username));
                userElement.Add(new XElement("MostComments", user.MostComments));

                xDoc.Root.Add(userElement);
            }

            var usersXml = xDoc.ToString();

            return usersXml;
        }
    }
}
