using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public interface IProdutosRepository
    {
        IQueryable<Produtos> GetAll();
        Produtos Get(int id);
        Produtos Create(Produtos produtos);
        bool Update(Produtos produtos);
        bool Delete(int id);
    }
}
