using Microsoft.AspNetCore.Mvc;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
            var produtos = _context.Produtos.AsNoTracking().Take(4).ToList(); 
            //Não é recomendável fazer uma consulta de todos os produtos de uma única vez.
            //Devido a isso, é usando o filtro Take() para especificar quantos produtos devemos retornar na consulta.
            //Utilizamos o método AsNoTracking() para que a consulta não seja rastreada pelo compilador, fazendo com que ela não seja armazenada em cache, 
            //deixando o código com um melhor desempenho.
            if(produtos is null)
            {
                return NotFound("Desculpe, não enconstramos o produto...");
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name="ObterProduto")]
        public ActionResult<Produtos> Get(int id)
        {
            var produtos = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.Id == id);
            if (produtos is null)
            {
                return NotFound("Desculpe, não enconstramos o produto...");
            }
            return produtos;
        }

        [HttpPost]
        public ActionResult Post(Produtos produtoPost)
        {
            _context.Produtos.Add(produtoPost);
            _context.SaveChanges();

            if(produtoPost is null)
            {
                return BadRequest("Requisição inválida. Tente novamente.");
            }

            return new CreatedAtRouteResult("ObterProduto", new {id = produtoPost.Id}, produtoPost);
        }

        [HttpPut("{identification:int}")]

        public ActionResult Put(int identification, Produtos produtoPut)
        {

            if (identification != produtoPut.Id)
            {
                return BadRequest("Requisição inválida.");
            }

            _context.Entry(produtoPut).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produtoPut);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var produtosDelete = _context.Produtos.FirstOrDefault(p => p.Id == id);

            if (produtosDelete is null)
            {
                return NotFound("Produto não encontrado, verifique o ID informado");
            }

            _context.Produtos.Remove(produtosDelete);
            _context.SaveChanges();

            return Ok(produtosDelete);
        }
    }
}
