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
        readonly IProdutosRepository _repository;
        readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutosRepository repository, ILogger<ProdutosController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produtos>> Get()
        {
            var produtos = _repository.GetAll().Where(p => p.Id <= 6).ToList();

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
            var produtos = _repository.Get(id);
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
            _repository.Create(produtoPost);

            if (produtoPost is null)
            {
                _logger.LogWarning("Requisição nula não é aceita. Preencha os dados corretamente.");
                return BadRequest("Requisição nula não é aceita. Preencha os dados corretamente.");
            }

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
            bool update = _repository.Update(produtoPut);
            if (update)
            {
                return Ok(update);
            }
            return (StatusCode(500, $"Falha ao atualizar o produto de Id {identification}"));

        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            bool delete = _repository.Delete(id);
            if (delete)
            {
                return Ok($"O produto de Id {id} foi excluído.");
            }
            return StatusCode(500, $"Falha ao excluir o produto de Id {id}.");
        }
    }
}
