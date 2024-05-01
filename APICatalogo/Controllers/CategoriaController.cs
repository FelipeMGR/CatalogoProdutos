using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
       
        public ActionResult<IEnumerable<Categoria>> GetCategoriaProduto()
        {
            return _context.Categoria.Include(p => p.Produtos).ToList();
        } 

        [HttpGet]

        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categoria.ToList();
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]

        public ActionResult<Categoria> Get(int id)
        {
            var produto = _context.Categoria.FirstOrDefault(p => p.CategoriaId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado.");
            }

            return produto;
        }

        [HttpPost]

        public ActionResult Post(Categoria categoria)
        {
            _context.Categoria.Add(categoria);
            _context.SaveChanges();

            if (categoria is null)
            {
                return BadRequest("Requisição inválida");
            }

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }


        [HttpPut("{identification:int}")]

        public ActionResult Put(int identification, Categoria categoriaPut)
        {

            if (identification != categoriaPut.CategoriaId)
            {
                return BadRequest("Requisição inválida.");
            }

            _context.Entry(categoriaPut).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoriaPut);
        }

        [HttpDelete]

        public ActionResult Delete(int id)
        {
            var produtoDelete = _context.Categoria.FirstOrDefault(p => p.CategoriaId == id);

            if (produtoDelete is null)
            {
                return NotFound("Categoria não encontrada");
            }

            _context.Categoria.Remove(produtoDelete);
            _context.SaveChanges();

            return Ok(produtoDelete);
        }
    }
}
