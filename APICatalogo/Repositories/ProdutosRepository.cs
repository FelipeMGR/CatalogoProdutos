using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
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

        public PagedList<Produtos> GetProdutos(ProdutosParameters produtosParams)
        {
            //GetAll().OrderBy(p => p.Nome).Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize).Take(produtosParams.PageSize).ToList();
            var produtos = GetAll().OrderBy(p => p.Id).AsQueryable();
            var produtosOrdenas =  PagedList<Produtos>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);

            return produtosOrdenas;
        }

        public IEnumerable<Produtos> GetProdutosPorCategoria(int? id)
        {
            return GetAll().Where(x => x.CategoriaId == id);
        }
    }
}
