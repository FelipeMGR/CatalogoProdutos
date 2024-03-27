using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace APICatalogo.Models;

[Table("Categoria")]
public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produtos>();
    }

    [Key]
    public int CategoriaId { get; set; }
    
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    public ICollection<Produtos>? Produtos { get; set; }
}
