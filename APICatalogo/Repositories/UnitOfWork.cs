using APICatalogo.Context;

namespace APICatalogo.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProdutosRepository? _produtosRepository;
        private ICategoriaRepository? _categoriaRepository;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutosRepository ProdutosRepository
        {
            get
            {
                //Verifica se já existe uma instância do tipo IProdutosRepository criada. Caso tenha, não será criada mais nenhuma, mas, caso não tenha, será criada uma instância.
                //Desta forma, a instânciação dessas classes/interfaces serão feitas somente quando realmente necessário, e somente uma vez.
                //Esse padrão é conhecido como "Lazy Loading", que consiste em usar um recurso somente quando for de fato precisa utilizá-lo.
                return _produtosRepository = _produtosRepository ?? new ProdutosRepository(_context);
            }
        }
        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepository = _categoriaRepository ?? new CategoriasRepository(_context);
            }
        }
        public void Commit()
        {
            //Utilizamos este método para persistir as alterações realizadas, ao invés de deixar essa responsabilidade com os controladores.
            _context.SaveChanges();
        }

        public void Dispose()
        {
            //Método usado para liberar os recursos não gerenciados pela memória.
            _context.Dispose();
        }
    }
}
