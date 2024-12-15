using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;

namespace TaskManagement.Data
{
    public class WorkDbContext : DbContext
    {
        public WorkDbContext(DbContextOptions<WorkDbContext> options) : base(options)
        { 
        }

        public DbSet<Work> Works { get; set; }
        public DbSet<User> Users { get; set; }
    }
}