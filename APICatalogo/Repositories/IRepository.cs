using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public interface IRepository <T>
    {
        IEnumerable<T> GetAll();
        //Aqui, o código funcionará da seguinte forma:
        //Expression --> Permitirá que uma expressão lambda seja passada como parâmetro;
        //Func<T, bool> --> receberá um valor do tipo T e retornará true ou false, baseado no que for passado como predicado.
        //predicate -->  será a expressão passada para analise para o Func.
        T? Get(Expression<Func<T, bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
