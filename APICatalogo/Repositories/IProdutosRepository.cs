using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories
{
    public interface IProdutosRepository : IRepository<Produtos>
    {
        PagedList<Produtos> GetProdutos(ProdutosParameters produtosParams);
        //IEnumerable<Produtos> GetProdutos(ProdutosParameters produtosParams);
        IEnumerable<Produtos> GetProdutosPorCategoria(int? id);
    }
}
