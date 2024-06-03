using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriasRepository : ICategoriaRepository
    {
        readonly AppDbContext _context;

        public CategoriasRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Categoria> GetCategoriaProdutos()
        {
            var categs = _context.Categoria.Include(p => p.Produtos).Where(p => p.CategoriaId <= 5).AsNoTracking().ToList();

            return categs;
        }
        public IEnumerable<Categoria> GetAll()
        {
            return _context.Categoria.Take(4).AsNoTracking().ToList();
        }

        public Categoria Get(int id)
        {
            var categ = _context.Categoria.FirstOrDefault(c => c.CategoriaId == id);
            return categ;
        }

        public Categoria Create(Categoria categoria)
        {
            _context.Categoria.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }

        public Categoria Update(Categoria categoria)
        { 

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return categoria;
        }

        public Categoria Delete(int id)
        {
            
            var categ = _context.Categoria.Find(id);
            _context.Remove(categ);
            _context.SaveChanges();

            return categ;
        }
    }
}
