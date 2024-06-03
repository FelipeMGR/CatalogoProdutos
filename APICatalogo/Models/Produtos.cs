using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using APICatalogo.Models;
using System.Text.Json.Serialization;
using APICatalogo.Validations;

namespace APICatalogo.Models;

public class Produtos
{
    [Key]
    public int Id { get; set; }

    [Required]
    [NameValidation]
    public string? Nome { get; set; }

    [Required]
    public string? Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Preco { get; set; }

    [Required]
    public string? ImagemUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }
    public int CategoriaId { get; set; }
}
