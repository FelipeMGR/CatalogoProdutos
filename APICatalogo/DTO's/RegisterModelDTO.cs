using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO_s
{
    public class RegisterModelDTO // classe responsável por armazenar os dados do usuário
    {
        [Required(ErrorMessage = "Informe o nome de usuário/login!")]
        public string? Login { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Informe um email!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Informe sua senha!")]
        public string? Password { get; set; }
    }
}
