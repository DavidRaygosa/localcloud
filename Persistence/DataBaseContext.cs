using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class DataBaseContext : IdentityDbContext<User>
    {
        public DataBaseContext(DbContextOptions options) : base(options){
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FileModel>().HasKey(ci=>new {ci.FileID}); // TIENE 2 LLAVES PRIMARIAS
        }

        public DbSet<FileModel> File{get;set;}
        public DbSet<User> User{get;set;}

        public DbSet<Document> Document{get;set;}
    }
}