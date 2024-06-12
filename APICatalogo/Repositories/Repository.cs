using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public class Repository<T> : IRepository<T> where T : class

    {
        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            //Como esta lista esta sendo usada apenas para a exibição de itens, usar o AsNoTracking é válido.
            //Se tivesse algum tipo de modificação sendo feita juntamente com a lista, seria inviável usar o AsNoTracking, pois o EF não saberia o que fazer com as variaveis.
            return _context.Set<T>().AsNoTracking().ToList();
        }
        public T? Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }
        public T Create(T entity)
        {
            _context.Add(entity);
            //_context.SaveChanges();

            return entity;
        }
        public T Update(T entity)
        {
            //_context.Set<T>().Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            //_context.SaveChanges();

            return entity;
        }
        public T Delete(T entity)
        {

            _context.Set<T>().Remove(entity);
            //_context.SaveChanges();

            return entity;
        }
    }
}
