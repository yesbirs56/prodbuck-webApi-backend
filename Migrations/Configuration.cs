namespace WebApiBasicCrud.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApiBasicCrud.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApiBasicCrud.Data.ProductContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApiBasicCrud.Data.ProductContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            Category electronics = new Category()
            {
                Id = 1,
                Name = "Electronics"
            };
            Category homeAppliences = new Category()
            {
                Id = 2,
                Name = "Home Appliences"
            };
            Category fashion = new Category()
            {
                Id = 3,
                Name = "Fashion"
            };
            context.Categories.AddOrUpdate(c=>c.Id,electronics,homeAppliences,fashion);
        }
    }
}
