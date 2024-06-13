using Microsoft.AspNetCore.Mvc;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Filters;
using APICatalogo.Repositories;
using APICatalogo.DTO_s;
using APICatalogo.DTO_s.Mapping;
using AutoMapper;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : Controller
    {
        readonly IUnitOfWork _uof;
        readonly ILogger<ProdutosController> _logger;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger, IMapper mapper)
        {
            _logger = logger;
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<ProdutosDTO>> GetPorCategoria(int id)
        {
            var prod = _uof.ProdutosRepository.GetProdutosPorCategoria(id);
            if (prod is null)
            {
                return NotFound("Produtos não encontrados.");
            }
            //var destino = _mapper.Map<Retorno>(origem);
            var produtosDto = _mapper.Map<IEnumerable<ProdutosDTO>>(prod);
            return Ok(produtosDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutosDTO>> Get()
        {
            var produtos = _uof.ProdutosRepository.GetAll().Where(p => p.Id <= 6).ToList();

            if (produtos is null)
            {
                _logger.LogWarning("Produto não encontrado.");
                return NotFound("Desculpe, não enconstramos o produto...");
            }
            var produtosDto = _mapper.Map<IEnumerable<ProdutosDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<ProdutosDTO> Get(int id)
        {
            var produtos = _uof.ProdutosRepository.Get(c=> c.Id == id);
            if (produtos is null)
            {
                _logger.LogWarning("Produto não encontrado.");
                return NotFound("Desculpe, não enconstramos o produto...");
            }

            var prod = _mapper.Map<ProdutosDTO>(produtos);
            return Ok(prod);
            
        }

        [HttpPost]
        public ActionResult<ProdutosDTO> Post(ProdutosDTO produtoPost)
        {

            if (produtoPost is null)
            {
                _logger.LogWarning("Requisição nula não é aceita. Preencha os dados corretamente.");
                return BadRequest("Requisição nula não é aceita. Preencha os dados corretamente.");
            }
            var produto = _mapper.Map<Produtos>(produtoPost);
            _uof.ProdutosRepository.Create(produto);
            _uof.Commit();

            var novoProduto = _mapper.Map<ProdutosDTO>(produto);
            return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.Id }, novoProduto);
        }

        [HttpPut("{identification:int}")]
        public ActionResult<ProdutosDTO> Put(int identification, ProdutosDTO produtoPut)
        {
            if (identification != produtoPut.Id)
            {
                _logger.LogWarning($"O produto de Id {identification} não foi encontrado.");
                return BadRequest($"O produto de Id {identification} não foi encontrado.");
            }
            var produto = _mapper.Map<Produtos>(produtoPut);
            var update = _uof.ProdutosRepository.Update(produto);
            _uof.Commit();

            var produtoDto = _mapper.Map<ProdutosDTO>(update);
            return Ok(produtoDto);

        }

        [HttpDelete]
        public ActionResult<ProdutosDTO> Delete(ProdutosDTO produto)
        {

            if (produto is null)
            {
                return NotFound("Produto não localziado.");
            }
            var produtoDelete = _mapper.Map<Produtos>(produto);
            var delete = _uof.ProdutosRepository.Delete(produtoDelete);
            _uof.Commit();

            var produtoExcluido = _mapper.Map<ProdutosDTO>(delete);
            return Ok(produtoExcluido);
        }
    }
}
