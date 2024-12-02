using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibreraDes.Models;
using LibreraDes.Data;
using Microsoft.EntityFrameworkCore;

namespace LibreraDes.Servicios
{
    public class ServicioCategoriasAutores
    {
        private readonly LibreriaDbContext _context;

        public ServicioCategoriasAutores(LibreriaDbContext context)
        {
            _context = context;
        }

        // Obtener todas las categorías
        public async Task<(bool isSuccess, List<Categoria> categorias, string errorMessage)> ObtenerTodasLasCategorias()
        {
            try
            {
                var categorias = await _context.Categorias.ToListAsync();
                return (true, categorias, null);
            }
            catch (Exception ex)
            {
                return (false, null, $"Error al obtener categorías: {ex.Message}");
            }
        }

        // Obtener una categoría por Id
        public async Task<(bool isSuccess, Categoria categoria, string errorMessage)> ObtenerCategoriaPorId(int id)
        {
            try
            {
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null)
                    return (false, null, "Categoría no encontrada.");
                return (true, categoria, null);
            }
            catch (Exception ex)
            {
                return (false, null, $"Error al obtener la categoría: {ex.Message}");
            }
        }

        // Obtener todos los autores
        public async Task<(bool isSuccess, List<Autor> autores, string errorMessage)> ObtenerTodosLosAutores()
        {
            try
            {
                var autores = await _context.Autores.Include(a => a.Libros).ToListAsync();
                return (true, autores, null);
            }
            catch (Exception ex)
            {
                return (false, null, $"Error al obtener autores: {ex.Message}");
            }
        }

        // Obtener un autor por Id
        public async Task<(bool isSuccess, Autor autor, string errorMessage)> ObtenerAutorPorId(int id)
        {
            try
            {
                var autor = await _context.Autores.Include(a => a.Libros).FirstOrDefaultAsync(a => a.Id == id);
                if (autor == null)
                    return (false, null, "Autor no encontrado.");
                return (true, autor, null);
            }
            catch (Exception ex)
            {
                return (false, null, $"Error al obtener el autor: {ex.Message}");
            }
        }

        // Insertar una nueva categoría
        public async Task<(bool isSuccess, string errorMessage)> InsertarCategoria(Categoria nuevaCategoria)
        {
            try
            {
                _context.Categorias.Add(nuevaCategoria);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al insertar la categoría: {ex.Message}");
            }
        }

        // Insertar un nuevo autor
        public async Task<(bool isSuccess, string errorMessage)> InsertarAutor(Autor nuevoAutor)
        {
            try
            {
                _context.Autores.Add(nuevoAutor);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al insertar el autor: {ex.Message}");
            }
        }

        // Modificar una categoría existente
        public async Task<(bool isSuccess, string errorMessage)> ModificarCategoria(Categoria categoria)
        {
            try
            {
                _context.Categorias.Update(categoria);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al modificar la categoría: {ex.Message}");
            }
        }

        // Modificar un autor existente
        public async Task<(bool isSuccess, string errorMessage)> ModificarAutor(Autor autor)
        {
            try
            {
                _context.Autores.Update(autor);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al modificar el autor: {ex.Message}");
            }
        }

        // Eliminar una categoría
        public async Task<(bool isSuccess, string errorMessage)> EliminarCategoria(int categoriaId)
        {
            try
            {
                var categoria = await _context.Categorias.FindAsync(categoriaId);
                if (categoria == null)
                    return (false, "Categoría no encontrada.");

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar la categoría: {ex.Message}");
            }
        }

        // Eliminar un autor
        public async Task<(bool isSuccess, string errorMessage)> EliminarAutor(int autorId)
        {
            try
            {
                var autor = await _context.Autores.FindAsync(autorId);
                if (autor == null)
                    return (false, "Autor no encontrado.");

                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar el autor: {ex.Message}");
            }
        }
    }
}
