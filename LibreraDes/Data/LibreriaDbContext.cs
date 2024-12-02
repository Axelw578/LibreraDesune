using LibreraDes.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreraDes.Data
{
    public class LibreriaDbContext : DbContext
    {
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetallesVentas> DetallesVentas { get; set; }
        public DbSet<Bitacora> Bitacoras { get; set; }

        // Constructor que acepta opciones de configuración
        public LibreriaDbContext(DbContextOptions<LibreriaDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones y restricciones adicionales
            modelBuilder.Entity<Autor>()
                .HasMany(a => a.Libros)
                .WithOne(l => l.Autor)
                .HasForeignKey(l => l.AutorId);

            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Libros)
                .WithOne(l => l.Categoria)
                .HasForeignKey(l => l.CategoriaId);

            modelBuilder.Entity<Libro>()
                .HasMany(l => l.Carritos)
                .WithOne(c => c.Libro)
                .HasForeignKey(c => c.LibroId);

            modelBuilder.Entity<Venta>()
                .HasMany(v => v.DetallesVentas)
                .WithOne(dv => dv.Venta)
                .HasForeignKey(dv => dv.VentaId);
        }
    }
}
