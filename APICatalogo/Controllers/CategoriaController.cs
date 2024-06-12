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
        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriaController> _logger;

        public CategoriaController(ILogger<CategoriaController> logger, IUnitOfWork uof)
        {
            _logger = logger;
            _uof = uof;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var cat = _uof.CategoriaRepository.GetAll();
            return Ok(cat);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]

        public ActionResult<Categoria> Get(int id)
        {

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

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
            var categoriaNova = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

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
                _logger.LogWarning("Não foi possível encontrar este objeto.");
                return BadRequest("Requisição inválida.");
            }

            var update = _uof.CategoriaRepository.Update(categoriaPut);
            _uof.Commit();
            return Ok(update);
        }

        [HttpDelete]
        public ActionResult Delete(Categoria categoria)
        {
            var produtoDelete = _uof.CategoriaRepository.Delete(categoria);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria não encontrada.");
                return NotFound($"Categoria não encontrada.");
            }
            _uof.Commit();
            return Ok(produtoDelete);
        }
    }
}
