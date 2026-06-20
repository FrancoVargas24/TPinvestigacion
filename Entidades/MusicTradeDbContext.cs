using Microsoft.EntityFrameworkCore;
using Entidades.Seed;

namespace Entidades;

public class MusicTradeDbContext : DbContext
{
    // Único constructor: recibe las opciones (con el connection string ya armado)
    // desde el contenedor de DI de ASP.NET Core (ver Program.cs -> AddDbContext).
    public MusicTradeDbContext(DbContextOptions<MusicTradeDbContext> options) : base(options) { }


    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Provincia> Provincias { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Publicacion> Publicaciones { get; set; }
    public DbSet<Mensaje> Mensajes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=MusicTradeDB;User Id=sa;Password=Password123!;TrustServerCertificate=True;");
        //optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MusicTradeDB;User Id=sa;Password=Password123!;Trusted_Connection=True;TrustServerCertificate=True;");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
        .HasIndex(u => u.Email)
        .IsUnique();

        modelBuilder.Entity<Mensaje>()
            .HasOne(m => m.Usuario)
            .WithMany(u => u.Mensajes)
            .HasForeignKey(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<Mensaje>()
            .HasOne(m => m.Destinatario)
            .WithMany()
            .HasForeignKey(m => m.DestinatarioId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Mensaje>()
            .HasOne(m => m.Publicacion)
            .WithMany(p => p.Mensajes)
            .HasForeignKey(m => m.PublicacionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Publicacion>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Publicaciones)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Publicacion>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Publicaciones)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.Provincia)
            .WithMany(p => p.Usuarios)
            .HasForeignKey(u => u.ProvinciaId)
            .OnDelete(DeleteBehavior.NoAction);

        ProvinciasSeed.Seed(modelBuilder);

        

    }
}
