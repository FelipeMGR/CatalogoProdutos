using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public interface IProdutosRepository : IRepository<Produtos>
    {
        IEnumerable<Produtos> GetProdutosPorCategoria(int? id);
    }
}
