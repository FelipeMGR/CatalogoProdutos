using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Filters;
using APICatalogo.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using APICatalogo.DTO_s;

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
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var cat = _uof.CategoriaRepository.GetAll();
            if (cat is null)
                return BadRequest("Categoria n]ao encontrada.");
          
            var categorias = cat.ToEnumrableDTO();
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria de Id {id} não encontrada");
                return NotFound($"Categoria de Id {id} não encontrada");
            }

            var categoriaDto = categoria.ToCategoriaDTO();
            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Requisição inválida.");
                return BadRequest("Requisição inválida");
            }
            var categoria = categoriaDto.ToCategoria();
            //cria a nova categoria usando como base os dados passados na categoriaDTO, e que foram convertidos em Categoria anteriormente.
            var categoriaNova = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();
            var novaCategoria = categoriaNova.ToCategoriaDTO();
            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoria.CategoriaId }, novaCategoria);
        }


        [HttpPut("{identification:int}")]
        public ActionResult<CategoriaDTO> Put(int identification, CategoriaDTO categoriaPut)
        {

            if (identification != categoriaPut.CategoriaId)
            {
                _logger.LogWarning("Não foi possível encontrar este objeto.");
                return BadRequest("Requisição inválida.");
            }
            var categoriaUpdate = categoriaPut.ToCategoria();

            var update = _uof.CategoriaRepository.Update(categoriaUpdate);
            _uof.Commit();

            var atualizada = categoriaUpdate.ToCategoriaDTO();
            return Ok(atualizada);
        }

        [HttpDelete]
        public ActionResult<CategoriaDTO> Delete(CategoriaDTO categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria não encontrada.");
                return NotFound($"Categoria não encontrada.");
            }
            var categoriaExcluida = categoria.ToCategoria();
            var produtoDelete = _uof.CategoriaRepository.Delete(categoriaExcluida);
            _uof.Commit();

            var categoriaExcluidaDTO = produtoDelete.ToCategoriaDTO();
            return Ok(categoriaExcluidaDTO);
        }
    }
}
