using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibreraDes.Models;
using LibreraDes.Data;
using Microsoft.EntityFrameworkCore;

namespace LibreraDes.Servicios
{
    public class ServicioLibros
    {
        private readonly LibreriaDbContext _context;

        public ServicioLibros(LibreriaDbContext context)
        {
            _context = context;
        }

        // Obtener todos los libros
        public async Task<List<Libro>> ObtenerTodosLosLibros()
        {
            return await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .ToListAsync();
        }

        // Obtener libros por categoría
        public async Task<List<Libro>> ObtenerLibrosPorCategoria(int categoriaId)
        {
            return await _context.Libros
                .Where(l => l.CategoriaId == categoriaId)
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .ToListAsync();
        }

        // Obtener libros por autor
        public async Task<List<Libro>> ObtenerLibrosPorAutor(int autorId)
        {
            return await _context.Libros
                .Where(l => l.AutorId == autorId)
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .ToListAsync();
        }

        // Buscar libros por título
        public async Task<List<Libro>> BuscarLibrosPorTitulo(string titulo)
        {
            return await _context.Libros
                .Where(l => l.Titulo.Contains(titulo))
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .ToListAsync();
        }

        // Crear un nuevo libro
        public async Task CrearLibro(Libro nuevoLibro)
        {
            await _context.Libros.AddAsync(nuevoLibro);
            await _context.SaveChangesAsync();
        }

        // Actualizar un libro existente
        public async Task ActualizarLibro(Libro libroModificado)
        {
            _context.Libros.Update(libroModificado);
            await _context.SaveChangesAsync();
        }

        // Eliminar un libro
        public async Task EliminarLibro(int libroId)
        {
            var libro = await ObtenerLibroPorId(libroId);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
            }
        }

        // Obtener libros comprados por un usuario
        public List<Libro> ObtenerLibrosCompradosPorUsuario(int usuarioId)
        {
            var ventas = _context.Ventas
                                 .Include(v => v.DetallesVentas)
                                 .ThenInclude(dv => dv.Libro)
                                 .Where(v => v.UsuarioId == usuarioId)
                                 .ToList();

            var librosComprados = ventas.SelectMany(v => v.DetallesVentas)
                                        .Select(dv => dv.Libro)
                                        .ToList();

            return librosComprados;
        }

        // Obtener un libro por su ID
        public async Task<Libro> ObtenerLibroPorId(int id)
        {
            return await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
