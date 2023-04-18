using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        public PizzaContext(DbContextOptions<PizzaContext> options) : base(options) { }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public void Seed()
        {

           
                var pizzaSeed = new Pizza[]
                {
            new Pizza
            {
                Name = "Pizza Margherita",
                Description = "Pizza classica dal gusto inconfondibile.",
                CategoryId = 1,
                Image = "img/margherita-50kalo.jpg",
                Price = 4,
            },
            new Pizza
            {
                Name = "Americana",
                Description = "Pizza che De Sica definirebbe con: 'Che è sta cafonata'.",
                CategoryId = 2,
                Image = "img/americana.jpg",
                Price = 5,
            },
            new Pizza
            {
                Name = "Diavola",
                Description = "Pizza leggera ma quel pizzico in più.",
                CategoryId = 1,
                Image = "img/diavola.jpg",
                Price = 4.5,
            },
            new Pizza
            {
                Name = "Capricciosa",
                Description = "Pizza amata dal ristoratore. Gli permette di liberarsi dei condimenti",
                CategoryId = 3,
                Image = "img/Capricciosa.jpg",
                Price = 6.5,
            }
            };
            if (!Pizzas.Any())
            {
                Pizzas.AddRange(pizzaSeed);

            }

            if (!Categories.Any())
                {
                var seed = new Category[]
                  {
                      new Category
                      {
                          Title = "Pizze classiche",
                      },
                      new Category
                      {
                          Title = "Pizze bianche",
                      },
                      new Category
                      {
                          Title = "Pizze vegetariane",
                      },
                      new Category
                      {
                          Title = "Pizze di mare",
                      }
                  };
                Categories.AddRange(seed);
            }

            if (!Ingredients.Any())
            {
                var seed = new Ingredient[]
                {
                    new()
                    {
                        Name = "Farina",
                    },
                    new()
                    {
                        Name = "Mozzarella",
                    },
                    new()
                    {
                        Name = "Pomodoro",
                        Pizzas = pizzaSeed
                    },
                    new()
                    {
                        Name = "Patate",
                    },
                };
                Ingredients.AddRange(seed);
            }

            if (!Roles.Any())
            {
                var seed = new IdentityRole[]
                {
                    new("Admin"),
                    new("User")
                };

                Roles.AddRange(seed);
            }

            if(Users.Any(u => u.Email == "admin@dev.com" || u.Email == "user@dev.com") && !UserRoles.Any() )
            {
                var admin = Users.First(u => u.Email == "admin@dev.com");  
                var user = Users.First(u => u.Email == "user@dev.com");

                var adminRole = Roles.First(r => r.Name == "Admin");
                var userRole = Roles.First(r => r.Name == "User");

                var seed = new IdentityUserRole<string>[]
                {
                    new()
                    {
                    UserId = admin.Id,
                    RoleId = adminRole.Id
                    },
                    new()
                    {
                    UserId = user.Id,
                    RoleId = userRole.Id
                    },
                };

                UserRoles.AddRange(seed);
            }

                SaveChanges();
        }
    }
}

