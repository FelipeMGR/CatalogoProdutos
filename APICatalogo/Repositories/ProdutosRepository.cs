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

        public PagedList<Produtos> GetProdutosFiltro(ProdutosFiltroPreco produtosFiltro)
        {
            var produtos = GetAll().AsQueryable();

            if (produtosFiltro.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltro.PrecoCriterio))
            {
                if (produtosFiltro.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltro.Preco.Value).OrderBy(p => p.Preco);
                }
                if (produtosFiltro.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltro.Preco.Value).OrderBy(p => p.Preco);
                }
                else if(produtosFiltro.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltro.Preco.Value).OrderBy(p => p.Preco);
                }
            }

            var produtosFiltrados = PagedList<Produtos>.ToPagedList(produtos, produtosFiltro.PageNumber, produtosFiltro.PageSize);

            return produtosFiltrados;
        }

        public IEnumerable<Produtos> GetProdutosPorCategoria(int? id)
        {
            return GetAll().Where(x => x.CategoriaId == id);
        }
    }
}
