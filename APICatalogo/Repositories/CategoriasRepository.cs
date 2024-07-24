using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriasRepository : Repository<Categoria>, ICategoriaRepository
    {
        readonly AppDbContext _context;

        public CategoriasRepository(AppDbContext context) : base(context) { 
        }
        public PagedList<Categoria> GetCategoria(CategoriaParameters categoriaParams)
        {
            //GetAll().OrderBy(p => p.Nome).Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize).Take(produtosParams.PageSize).ToList();
            var categoria = GetAll().OrderBy(p => p.CategoriaId).AsQueryable();
            var categoriasOrdenas = PagedList<Categoria>.ToPagedList(categoria, categoriaParams.PageNumber, categoriaParams.PageSize);

            return categoriasOrdenas;
        }

        public PagedList<Categoria> GetCategorias(CategoriaParameters categoriasParams)
        {
            throw new NotImplementedException();
        }
    }
}
