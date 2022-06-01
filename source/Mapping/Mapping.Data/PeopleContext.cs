using Microsoft.EntityFrameworkCore;

namespace Mapping.Data;
public class PeopleContext : DbContext
{
    public DbSet<Person>? People { get; set; }

    public PeopleContext()
    {

    }

    public PeopleContext(DbContextOptions<PeopleContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=people.db;");
        base.OnConfiguring(optionsBuilder); 
    }
}
