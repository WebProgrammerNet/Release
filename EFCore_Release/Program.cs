using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace EFCore_Release
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (AppDbContext _db = new AppDbContext())
            {
                _db.Database.EnsureCreated();
                _db.Blogs.Add(new Blog()
                {
                    name = "dotnet",
                    Posts = new List<Post>()
                    {
                        new Post(){ Title = "C#", Content="C#8 has new features Switch Cases"}
                    },
                   // Url = "http://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties" http error verdi
                    Url = "https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties"
                });
                await _db.SaveChangesAsync();
                Console.WriteLine("Done");
            }

        }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SqlConnectionStringBuilder StringConnection = new SqlConnectionStringBuilder()
            {
                DataSource = "DESKTOP-NCRI8U1",
                InitialCatalog = "efrelease1",
                IntegratedSecurity = true,
            };
            optionsBuilder.UseSqlServer(StringConnection.ToString());
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Blog>().Property<string>("url");
            //builder.Entity<Blog>().Property(r => r.Url).HasField("url");
            
            base.OnModelCreating(builder);
        }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }

        [ForeignKey(nameof(BlogId))]
        public virtual Blog Blog { get; set; }

    }
    public class Blog
    {
        public int Id { get; set; }
        public string name { get; set; }
        private string url { get; set; }
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                if (value.StartsWith("https://"))
                {
                    url = value;
                }
                else
                {
                    throw new Exception(" Error - Url doesn't must negin with https:// ");
                }
            }
        }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
