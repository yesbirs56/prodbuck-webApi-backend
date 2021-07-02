using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

using WebApiBasicCrud.Models;
namespace WebApiBasicCrud.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext():base("name=Products")
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}