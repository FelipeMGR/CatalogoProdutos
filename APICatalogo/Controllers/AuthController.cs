using APICatalogo.DTO_s;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //define uma instância de TokenService.
        private readonly ITokenService _tokenService;
        //lida com os dados do usuario.
        private readonly UserManager<ApplicationUser> _userManager;
        //lida com os perfis definidos na API.
        private readonly RoleManager<ApplicationUser> _roleManager;
        //lida com as informações especificadas no appsetings.json
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService, 
                              UserManager<ApplicationUser> userManager, 
                              RoleManager<ApplicationUser> roleManager, 
                              IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO loginModel)
        {
            //identifica o usuario que está realizando o login. A busca é feita através do nome.
            var user = await _userManager.FindByNameAsync(loginModel.Login!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, loginModel.Password!))
            {
                //obtem os perfis associados ao usuario que está fazendo login
                var userRoles = await _userManager.GetRolesAsync(user);

                //cria uma lista de claims do user.
                var authCalim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    //atribui um código de identificação único ao token.
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var role in userRoles)
                {
                    authCalim.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = _tokenService.GenerateAccessToken(authCalim, _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidInMinutes"], out int refreshTokenExpireTime);

                user.RefreshTokenExpireTime = DateTime.Now.AddMinutes(refreshTokenExpireTime);

                user.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(user);

                return Ok(
                    new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = refreshToken,
                        Expiration = token.ValidTo
                    }
                 );
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDTO registerDTO)
        {
            var userExists = _userManager.FindByEmailAsync(registerDTO.Login!);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Failed",
                    Message = "Usuário já existe."
                });
            }

            ApplicationUser user = new()
            {
                UserName = registerDTO.Login,
                Email = registerDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Failed",
                    Message = "Criação do usuário falhou."
                });
            }
            return Ok(new Response
            {
                Status = "Succeeded",
                Message = "Criação de usuário concluída!"
            });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel model)
        {
            if (model == null)
            {
                return BadRequest("Requisição inválida.");
            }

            var acessToken = model.AcessToken ?? throw new ArgumentNullException(nameof(TokenModel));
            var refreshToken = model.RefreshToken ?? throw new ArgumentNullException(nameof(TokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!, _configuration);

            if (principal == null)
            {
                return BadRequest("Requisição inválida / Token inválido");
            }

            string? userName = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName!);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime
                < DateTime.UtcNow)
            {
                return BadRequest("Requisição inválida");
            }

            var newAcessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                acessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Revoke(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return BadRequest("O usuário não existe");
            }

            user.RefreshToken = null;

            return NoContent();
        }
    }
}
