

namespace ProductApi.DAL
{
    public class ApplicationDbContext : AuditableIdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<MailConfirmation> MailConfirmations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            builder.Entity<IdentityRole>().HasData(new IdentityRole("Admin"), new IdentityRole("User"));
        }


        public static async Task SeedData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,
            IProductService productService)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));

            await userManager.CreateAsync(new AppUser { UserName = "Admin", Email = "admin@gmail.com" }, 
                                                        "Secret123$");
            var user = await userManager.FindByNameAsync("Admin");
            userManager.AddToRoleAsync(user, "Admin");
            userManager.AddToRoleAsync(user, "User");

            new List<Product>
            {
                new Product{ Id=1, Name = "Product 1", Price = 120, Discount = 2},
                new Product{ Id=2, Name = "Product 2", Price = 120, Discount = 2},
                new Product{ Id=3, Name = "Product 3", Price = 120, Discount = 2},
                new Product{ Id=4, Name = "Product 4", Price = 120, Discount = 2},
                new Product{ Id=5, Name = "Product 5", Price = 120, Discount = 2},
                new Product{ Id=6, Name = "Product 6", Price = 120, Discount = 2},
                new Product{ Id=7, Name = "Product 7", Price = 120, Discount = 2},
                new Product{ Id=8, Name = "Product 8", Price = 120, Discount = 2},
                new Product{ Id=9, Name = "Product 9", Price = 120, Discount = 2},
                new Product{ Id=10, Name = "Product 10", Price = 120, Discount = 2},
                new Product{ Id=11, Name = "Product 11", Price = 120, Discount = 2},
                new Product{ Id=12, Name = "Product 12", Price = 120, Discount = 2},
                new Product{ Id=13, Name = "Product 13", Price = 120, Discount = 2},
                new Product{ Id=14, Name = "Product 14", Price = 120, Discount = 2},
                new Product{ Id=15, Name = "Product 15", Price = 120, Discount = 2},
                new Product{ Id=16, Name = "Product 16", Price = 120, Discount = 2},
                new Product{ Id=17, Name = "Product 17", Price = 120, Discount = 2},
                new Product{ Id=18, Name = "Product 18", Price = 120, Discount = 2},
                new Product{ Id=19, Name = "Product 19", Price = 120, Discount = 2},
                new Product{ Id=20, Name = "Product 20", Price = 120, Discount = 2},
            }.ForEach(x => productService.CreateProductWithoutAudit(x));




        }
    }
}