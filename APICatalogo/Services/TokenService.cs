using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;


namespace APICatalogo.Services
{
    public class TokenService : ITokenService
    {
        //class responsável pela criação do token de acesso.
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            //obtem a chave secreta que foi definida seção "SecretKey", em appsetting.json. A instância de IConfiguration nos permitem acessar essa seção.
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ?? throw new InvalidOperationException("Chave inválida.");

            //Faz a conversão da chave secreta para uma sequência em bytes.
            var privateKey = Encoding.UTF8.GetBytes(key);

            //Cria as credencias de assinatura do emissor do Token. As credencias utilizam a chave secreta, em bytes, e passadas como argumento no método SymmetricSecurityKey
            var signingCredential = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //esta seção especifica as informações que serão validadas pelo token
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT").GetValue<double>("TokenValidInMinutes")),

                Audience = _config.GetSection("JWT").GetValue<string>("Audience"),
                Issuer = _config.GetSection("JWT").GetValue<string>("Issuer"),
                SigningCredentials = signingCredential,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            //as variaveis acima trabalham juntas para criar o token de acesso, ambas são necessárias para a criação dele.
            return token;
        }
        //classe responsável pela atualização do token de acesso, fazendo com o que o usuário não precise informar suas credencias novamente.
        public string GenerateRefreshToken()
        {
            //especifica o tamanho do array de bytes
            var secureRandomBytes = new byte[128];

            //serve como instancia da classe RandomNumberGenerator, e do metodo Create() da classe.
            using var randomNumberGenerator = RandomNumberGenerator.Create();

            //preenche o array com uma quantidade de bytes, de forma aleatoria, mas segura.
            randomNumberGenerator.GetBytes(secureRandomBytes);

            //converte o array de bytes em uma sequência de string, para facilitar a leitura.
            var refreshToken = Convert.ToBase64String(secureRandomBytes);

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            //armazena a chave secreta
            var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Chave inválida.");
            //define os parametros para validação do token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false
            };
            //declara uma instância da classe JWTSecurityTokenHandler, para podermos manipular o token.
            var tokenHandler = new JwtSecurityTokenHandler();

            //Faz a validação do token com base nos parametros passados anteriormente.
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            //Verifica se a saída do token está de acordo com o padrão que foi estabelecido.
            //Se o tipo de retorno do token for diferente, ou o algoritmo dele não for o especificado, será lançada uma exceção.
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                                                                                                 StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException("Chave inválida");
            }
            //Retorna o token re-criado e já validado.  
            return principal;
        }
    }
}
