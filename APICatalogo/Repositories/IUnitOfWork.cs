namespace APICatalogo.Repositories
{
    public interface IUnitOfWork
    {
        IProdutosRepository ProdutosRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        void Commit();
    }
}
