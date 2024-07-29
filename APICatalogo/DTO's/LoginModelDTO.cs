using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO_s
{
    public class LoginModelDTO //classe responsável pelo login do usuário
    {
        [Required(ErrorMessage = "Informe o nome de usuário/login!")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Informe sua senha!")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Informe sua função.")]
        public string? Role { get; set; }
    }
}
