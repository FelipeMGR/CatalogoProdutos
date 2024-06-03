using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class ProdutosRepository : IProdutosRepository
    {

        readonly AppDbContext _context;

        public ProdutosRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Produtos> GetAll()
        {
            return _context.Produtos;
        }
        public Produtos Get(int id)
        {
            var prod = _context.Produtos.FirstOrDefault(p => p.Id == id);
            return prod;
        }
        public Produtos Create(Produtos produtos)
        {
            if (produtos is null)
                throw new ArgumentNullException(nameof(produtos));

            _context.Produtos.Add(produtos);
            _context.SaveChanges();
            return produtos;
        }
        public bool Update(Produtos produtos)
        {
            if (produtos is null)
                throw new ArgumentNullException(nameof(produtos));

            if(_context.Produtos.Any(p => p.Id == produtos.Id))
            {
                _context.Produtos.Update(produtos);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool Delete(int id)
        {
            var prod = _context.Produtos.Find(id);

            if (prod is null)
            {
                return false;
            }

            _context.Produtos.Remove(prod);
            _context.SaveChanges();
            return true;
                
        }
    }
}
