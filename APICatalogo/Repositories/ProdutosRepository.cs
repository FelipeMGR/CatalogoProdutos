using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public class ProdutosRepository : Repository<Produtos>, IProdutosRepository
    {

        readonly AppDbContext _context;

        public ProdutosRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Produtos> GetProdutosPorCategoria(int? id)
        {
            return GetAll().Where(x => x.CategoriaId == id);
        }
    }
}
