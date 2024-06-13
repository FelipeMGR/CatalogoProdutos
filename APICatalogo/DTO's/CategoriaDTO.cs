using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO_s
{
    public class CategoriaDTO
    {
        public int CategoriaId { get; set; }
        [Required]
        [NameValidation]
        public string? Nome { get; set; }
        [Required]
        public string? ImagemUrl { get; set; }
    }
}
