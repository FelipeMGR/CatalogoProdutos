using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using APICatalogo.Validations;
using System.Text.Json.Serialization;
namespace APICatalogo.Models;

public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produtos>();
    }

    [Key]
    public int CategoriaId { get; set; }
    
    [Required]
    [NameValidation]
    public string? Nome { get; set; }

    [Required]
    public string? ImagemUrl { get; set; }
    [JsonIgnore]
    public ICollection<Produtos>? Produtos { get; set; }
}
