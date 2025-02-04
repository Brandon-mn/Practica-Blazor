using Microsoft.EntityFrameworkCore;
using MudBlazorWebApp1.Models;

namespace MudBlazorWebApp1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TicketTemplate> TicketTemplates2 { get; set; }
        public DbSet<TicketElement> TicketElements { get; set; }
        public DbSet<DatosEvento> DatosEventos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TicketTemplate>()
                .HasMany(t => t.Elements)
                .WithOne()
                .HasForeignKey(e => e.TicketTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketElement>()
                .Property(e => e.PosX)
                .HasColumnType("float");

            modelBuilder.Entity<TicketElement>()
                .Property(e => e.PosY)
                .HasColumnType("float");

            modelBuilder.Entity<TicketElement>()
                .Property(e => e.Width)
                .HasColumnType("float");

            modelBuilder.Entity<TicketElement>()
                .Property(e => e.Height)
                .HasColumnType("float");

            modelBuilder.Entity<DatosEvento>()
                .ToTable("datos_evento") 
                .Property(e => e.Nombre)
                .HasMaxLength(255);

            modelBuilder.Entity<DatosEvento>()
                .Property(e => e.Temporada)
                .HasMaxLength(255);
        }

    }
}
