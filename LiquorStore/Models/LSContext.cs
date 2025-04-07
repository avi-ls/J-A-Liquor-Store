using System;
using Microsoft.EntityFrameworkCore;
using System.Data;
using LiquorStore.Models;

namespace LiquorStore.Models{
      public class LSContext:DbContext{
            public LSContext(DbContextOptions<LSContext> options) : base(options){}

            public DbSet<Product> Products { get; set; }
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<Employee> Employees { get; set; }
            public DbSet<User> User { get; set; }

    }
}
