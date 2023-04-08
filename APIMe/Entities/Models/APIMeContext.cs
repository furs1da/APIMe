using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using APIMe.Entities.Configuration;

namespace APIMe.Entities.Models
{
    public class APIMeContext : IdentityDbContext<IdentityUser>
    {

        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string username = "apimeconestoga@gmail.com";
                string password = "1OBb$^#0^u21!"; 
                string roleName = "Administrator";

                // if role doesn't exist, create it
                if (await roleManager.FindByNameAsync(roleName) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
                // if username doesn't exist, create it and add it to role
                if (await userManager.FindByNameAsync(username) == null)
                {
                    IdentityUser user = new IdentityUser();
                    user.EmailConfirmed = true;
                    user.Email = "apimeconestoga@gmail.com";
                    user.UserName = username;

                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
            }
        }
        public APIMeContext()
        {
        }

        public APIMeContext(DbContextOptions<APIMeContext> options)
            : base(options)
        {
        }

        //public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        //public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        //public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        //public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        //public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        //public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<BugFeature> BugFeatures { get; set; } = null!;
        public virtual DbSet<Customers> Customers { get; set; } = null!;
        public virtual DbSet<DeviceCode> DeviceCodes { get; set; } = null!;
        public virtual DbSet<Key> Keys { get; set; } = null!;
        public virtual DbSet<PersistedGrant> PersistedGrants { get; set; } = null!;
        public virtual DbSet<Products> Products { get; set; } = null!;
        public virtual DbSet<Professor> Professors { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Route> Routes { get; set; } = null!;

        public virtual DbSet<RouteLog> RouteLogs { get; set; } = null!;

        public virtual DbSet<RouteType> RouteTypes { get; set; } = null!;
        public virtual DbSet<Section> Sections { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentSection> StudentSections { get; set; } = null!;

        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Inventory> Inventories { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;

        public virtual DbSet<Order> Orders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=APIMe;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUserLogin>()
             .HasKey(l => new { l.LoginProvider, l.ProviderKey });

            modelBuilder.Entity<AspNetUserToken>()
           .HasKey(l => new { l.Name, l.LoginProvider, l.UserId });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BugFeature>(entity =>
            {
                entity.ToTable("BugFeature");

                entity.HasIndex(e => e.ProjectId, "IX_BugFeature_projectId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActualResults)
                    .HasMaxLength(200)
                    .HasColumnName("actualResults");

                entity.Property(e => e.Description)
                    .HasMaxLength(300)
                    .HasColumnName("description");

                entity.Property(e => e.Environment)
                    .HasMaxLength(100)
                    .HasColumnName("environment");

                entity.Property(e => e.ExpectedResults)
                    .HasMaxLength(200)
                    .HasColumnName("expectedResults");

                entity.Property(e => e.IsBug).HasColumnName("isBug");

                entity.Property(e => e.Priority)
                    .HasMaxLength(20)
                    .HasColumnName("priority");

                entity.Property(e => e.ProjectId).HasColumnName("projectId");

                entity.Property(e => e.SeverityId).HasColumnName("severityId");

                entity.Property(e => e.StepsToReproduce)
                    .HasMaxLength(200)
                    .HasColumnName("stepsToReproduce");

                entity.Property(e => e.Summary)
                    .HasMaxLength(100)
                    .HasColumnName("summary");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.BugFeatures)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BugFeature_Project");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.ToTable("Customers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");
            });




            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnName("price");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.ToTable("Professor");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(30)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(30)
                    .HasColumnName("lastName");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.HasIndex(e => e.UserId, "IX_Project_userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");

                entity.Property(e => e.Description)
                    .HasMaxLength(300)
                    .HasColumnName("description");

                entity.Property(e => e.IsCompleted).HasColumnName("isCompleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("name");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_AspNetUsers");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.ToTable("Route");

                entity.HasIndex(e => e.RouteTypeId, "IX_Route_routeTypeId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataTableName)
                    .HasMaxLength(30)
                    .HasColumnName("dataTableName");

                entity.Property(e => e.Description)
                    .HasMaxLength(300)
                    .HasColumnName("description");

                entity.Property(e => e.IsVisible).HasColumnName("isVisible");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.RouteTypeId).HasColumnName("routeTypeId");

                entity.HasOne(d => d.RouteType)
                    .WithMany(p => p.Routes)
                    .HasForeignKey(d => d.RouteTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Route_RouteType");
            });

            modelBuilder.Entity<RouteLog>(entity =>
            {
                entity.Property(e => e.FullName).HasMaxLength(256);

                entity.Property(e => e.HttpMethod).HasMaxLength(10);

                entity.Property(e => e.Ipaddress)
                    .HasMaxLength(45)
                    .HasColumnName("IPAddress");

                entity.Property(e => e.RequestTimestamp).HasColumnType("datetime");

                entity.Property(e => e.RoutePath).HasMaxLength(256);

                entity.Property(e => e.TableName).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RouteLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__RouteLogs__UserI__756D6ECB");
            });

            modelBuilder.Entity<RouteType>(entity =>
            {
                entity.ToTable("RouteType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CrudId).HasColumnName("crudId");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("name");

                entity.Property(e => e.ResponseCode)
                    .HasMaxLength(30)
                    .HasColumnName("responseCode");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.ToTable("Section");

                entity.HasIndex(e => e.ProfessorId, "IX_Section_professorId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccessCode)
                    .HasMaxLength(20)
                    .HasColumnName("accessCode");

                entity.Property(e => e.ProfessorId).HasColumnName("professorId");

                entity.Property(e => e.SectionName)
                    .HasMaxLength(25)
                    .HasColumnName("sectionName");

                entity.HasOne(d => d.Professor)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.ProfessorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Section_Professor");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApiKey)
                    .HasMaxLength(70)
                    .HasColumnName("apiKey");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(30)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .HasMaxLength(30)
                    .HasColumnName("lastName");

                entity.Property(e => e.StudentId).HasColumnName("studentId");
            });

            modelBuilder.Entity<StudentSection>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("StudentSection");

                entity.HasIndex(e => e.SectionId, "IX_StudentSection_sectionId");

                entity.HasIndex(e => e.StudentId, "IX_StudentSection_studentId");

                entity.Property(e => e.SectionId).HasColumnName("sectionId");

                entity.Property(e => e.StudentId).HasColumnName("studentId");

                entity.HasOne(d => d.Section)
                    .WithMany()
                    .HasForeignKey(d => d.SectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentSection_Section");

                entity.HasOne(d => d.Student)
                    .WithMany()
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentSection_Student");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Suppliers");


                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");
            });


            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });


            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                entity.Property(e => e.HireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("hire_date");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");
            });
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("Inventory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .HasColumnName("product_name");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.ReorderLevel).HasColumnName("reorder_level");

                entity.Property(e => e.UnitCost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("unit_cost");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(50)
                    .HasColumnName("customer_name");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date");

                entity.Property(e => e.OrderNumber)
                    .HasMaxLength(50)
                    .HasColumnName("order_number");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("total_amount");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(50)
                    .HasColumnName("customer_name");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("payment_date");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .HasColumnName("payment_method");
            });




            // composite primary key for StudentSection
            modelBuilder.Entity<StudentSection>()
                .HasKey(ce => new { ce.StudentId, ce.SectionId });

            // one-to-many relationship between Customer and CustomerEvent
            modelBuilder.Entity<StudentSection>()
                .HasOne(ce => ce.Student)
                .WithMany(c => c.StudentSections)
                .HasForeignKey(ce => ce.StudentId);

            // one-to-many relationship between Event and CustomerEvent
            modelBuilder.Entity<StudentSection>()
                .HasOne(ce => ce.Section)
                .WithMany(g => g.StudentSections)
                .HasForeignKey(ce => ce.SectionId);



            modelBuilder.ApplyConfiguration(new ProfessorConfiguration());
            modelBuilder.ApplyConfiguration(new SectionConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RouteTypeConfiguration());
        }
    }
}
