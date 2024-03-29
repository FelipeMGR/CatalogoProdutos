using Microsoft.AspNetCore.Mvc;
using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produtos>> Get()
        {
            var produtos = _context.Produtos.ToList();
            if(produtos is null)
            {
                return NotFound("Desculpe, não enconstramos o produto...");
            }
            return produtos;
        }

        [HttpGet("{id:int}")]
        public ActionResult<Produtos> Get(int id)
        {
            var produtos = _context.Produtos.FirstOrDefault(p => p.Id == id);
            if (produtos is null)
            {
                return NotFound("Desculpe, não enconstramos o produto...");
            }
            return produtos;
        }

        [HttpPost]

        public ActionResult Post(Produtos produto)
        {
            var produtos = _context.Produtos.Add(produto);
            _context.SaveChanges();

            if(produto is null)
            {
                return BadRequest("Requisição inválida. Tente novamente.");
            }

            return new CreatedAtRouteResult("ObterProduto",new {id = produto.Id} , produto);
        }
    }
}
