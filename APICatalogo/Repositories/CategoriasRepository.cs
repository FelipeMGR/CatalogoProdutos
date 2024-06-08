using APICatalogo.Context;
using APICatalogo.Models;
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
    }
}
