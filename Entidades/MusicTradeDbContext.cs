using Microsoft.EntityFrameworkCore;
using Entidades.Seed;

namespace Entidades;

public class MusicTradeDbContext : DbContext
{

    public MusicTradeDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=MusicTradeDB;User Id=sa;Password=Password123!;TrustServerCertificate=True;");
        //optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=TPPW3ConsorciosDB;User Id=sa;Password=Password123!;Trusted_Connection=True;TrustServerCertificate=True;");

        //optionsBuilder.UseSqlServer("Server=localhost,[Puerto];Database=[nombre_base_datos_no_cambiar];User Id=[usuario];Password=[contrasenia];TrustServerCertificate=True;");

    }


    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Provincia> Provincias { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Publicacion> Publicaciones { get; set; }
    public DbSet<Mensaje> Mensajes { get; set; }
    public DbSet<Oferta> Ofertas { get; set; }


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
            .HasOne(m => m.Publicacion)
            .WithMany(p => p.Mensajes)
            .HasForeignKey(m => m.PublicacionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Oferta>()
            .HasOne(o => o.Usuario)
            .WithMany(u => u.Ofertas)
            .HasForeignKey(o => o.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Oferta>()
            .HasOne(o => o.Publicacion)
            .WithMany(p => p.Ofertas)
            .HasForeignKey(o => o.PublicacionId)
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

        ProvinciasSeed.Seed(modelBuilder);

    }
}
