using APICatalogo.Models;
using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.DTO_s
{
    public class ProdutosDTO
    {
        public int Id { get; set; }
        [Required]
        [NameValidation]
        public string? Nome { get; set; }
        [Required]
        public string? Descricao { get; set; }
        [Required]
        public decimal Preco { get; set; }
        [Required]
        public string? ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
