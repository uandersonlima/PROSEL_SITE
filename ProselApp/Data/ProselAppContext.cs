using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProselApp.Models;
using ProselApp.Models.AcessCode;

namespace ProselApp.Data
{
    public class ProselAppContext : DbContext
    {
        public ProselAppContext(DbContextOptions<ProselAppContext> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<AccessCode> AccessCode { get; set; }
        public DbSet<Token> Token { get; set; }

    }
}