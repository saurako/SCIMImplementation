using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace SCIMAPI.Models
{
    public class UsersContextInitializer : DropCreateDatabaseIfModelChanges<UsersContext>
    {
        protected override void Seed(UsersContext context)
        {
            var users = new List<User> {
                new User {Id = Guid.NewGuid(), userName = "user1@contoso.com", displayName = "User One", active = true},
                new User {Id = Guid.NewGuid(), userName = "user2@contoso.com", displayName = "User Two", active = true},
                new User {Id = Guid.NewGuid(), userName = "user3@contoso.com", displayName = "User Three", active = false}
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();
        }
        

    }
}