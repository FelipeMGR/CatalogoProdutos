using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Filters;
using APICatalogo.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;
        private readonly ILogger<CategoriaController> _logger;

        public CategoriaController(ICategoriaRepository repository, ILogger<CategoriaController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("produtos")]
       
        public ActionResult<IEnumerable<Categoria>> GetCategoriaProduto()
        {
            var categorias = _repository.GetCategoriaProdutos();
            return Ok(categorias);
            //Não é recomendável incluir todos os produtos de uma vez, visto que, em cenários de maior quantidade, isso pode causar problemas de desempenho.
            //Nesses casos, o ideal é fazer um filtro antes de fazer a inclusão, colocando somente aquilo que realmente importa.
        } 

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var cat = _repository.GetAll();
            return Ok(cat);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]

        public ActionResult<Categoria> Get(int id)
        {
            
            var categoria = _repository.Get(id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria de Id {id} não encontrada");
                return NotFound($"Categoria de Id {id} não encontrada");
            }
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            var categoriaNova = _repository.Create(categoria);

            if (categoria is null)
            {
                _logger.LogWarning("Requisição inválida.");
                return BadRequest("Requisição inválida");
            }


            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaNova.CategoriaId }, categoriaNova);
        }


        [HttpPut("{identification:int}")]
        public ActionResult Put(int identification, Categoria categoriaPut)
        {

            if (identification != categoriaPut.CategoriaId)
            {
                _logger.LogWarning("Não é possível encontrar este objeto.");
                return BadRequest("Requisição inválida.");
            }

            var update = _repository.Update(categoriaPut);
            return Ok(update);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var produtoDelete = _repository.Delete(id);

            if (id != produtoDelete.CategoriaId)
            {
                _logger.LogWarning($"Categoria de Id {id} n]ao encontrada.");
                return NotFound($"Categoria de Id {id} n]ao encontrada.");
            }

            return Ok(produtoDelete);
        }
    }
}
