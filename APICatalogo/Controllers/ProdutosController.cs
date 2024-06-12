using Microsoft.AspNetCore.Mvc;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Filters;
using APICatalogo.Repositories;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : Controller
    {
        readonly IUnitOfWork _uof;
        readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
        {
            _logger = logger;
            _uof = uof;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produtos>> GetPorCategoria(int id)
        {
            var prod = _uof.ProdutosRepository.GetProdutosPorCategoria(id);
            if (prod is null)
            {
                return NotFound("Produtos não encontrados.");
            }

            return Ok(prod);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produtos>> Get()
        {
            var produtos = _uof.ProdutosRepository.GetAll().Where(p => p.Id <= 6).ToList();

            if (produtos is null)
            {
                _logger.LogWarning("Produto não encontrado.");
                return NotFound("Desculpe, não enconstramos o produto...");
            }
            return Ok(produtos);
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<Produtos> Get(int id)
        {
            var produtos = _uof.ProdutosRepository.Get(c=> c.Id == id);
            if (produtos is null)
            {
                _logger.LogWarning("Produto não encontrado.");
                return NotFound("Desculpe, não enconstramos o produto...");
            }
            return Ok(produtos);
            
        }

        [HttpPost]
        public ActionResult Post(Produtos produtoPost)
        {

            if (produtoPost is null)
            {
                _logger.LogWarning("Requisição nula não é aceita. Preencha os dados corretamente.");
                return BadRequest("Requisição nula não é aceita. Preencha os dados corretamente.");
            }

            _uof.ProdutosRepository.Create(produtoPost);
            _uof.Commit();
            

            return new CreatedAtRouteResult("ObterProduto", new { id = produtoPost.Id }, produtoPost);
        }

        [HttpPut("{identification:int}")]
        public ActionResult Put(int identification, Produtos produtoPut)
        {
            if (identification != produtoPut.Id)
            {
                _logger.LogWarning($"O produto de Id {identification} não foi encontrado.");
                return BadRequest($"O produto de Id {identification} não foi encontrado.");
            }

            var update = _uof.ProdutosRepository.Update(produtoPut);
            _uof.Commit();

            return Ok(update);

        }

        [HttpDelete]
        public ActionResult Delete(Produtos produto)
        {

            if (produto is null)
            {
                return NotFound("Produto não localziado.");
            }

            var delete = _uof.ProdutosRepository.Delete(produto);
            _uof.Commit();

            return Ok();
        }
    }
}
