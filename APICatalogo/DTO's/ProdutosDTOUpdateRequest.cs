using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.DTO_s
{
    public class ProdutosDTOUpdateRequest
    {
        [Range(1, 9999, ErrorMessage = "O estoque deve ter um valor mínimo de 1 e um máximo de 9999")]
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataCadastro <= DateTime.Now.Date)
            {
                yield return new ValidationResult("Data precisa ser maior do que a atual", [nameof(DataCadastro)]);
            }
        }
    }
}
