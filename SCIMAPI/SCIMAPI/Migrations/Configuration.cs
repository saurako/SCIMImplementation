namespace SCIMAPI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using SCIMAPI.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SCIMAPI.Models.UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SCIMAPI.Models.UsersContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var users = new List<User> {
                new User {Id = Guid.NewGuid(), userName = "user1@contoso.com", displayName = "User One", active = true},
                new User {Id = Guid.NewGuid(), userName = "user2@contoso.com", displayName = "User Two", active = true},
                new User {Id = Guid.NewGuid(), userName = "user3@contoso.com", displayName = "User Three", active = false}
            };

            users.ForEach(u => context.Users.AddOrUpdate(u));
            context.SaveChanges();
        }
    }
}
