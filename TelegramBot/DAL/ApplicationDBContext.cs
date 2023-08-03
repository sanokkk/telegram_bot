using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace TelegramBot.DAL;

public class ApplicationDBContext: DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
    :base(options)
    {
    }

    public DbSet<Domain.Models.User> Users { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Domain.Models.User>().ToTable("users");
    }
}