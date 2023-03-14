using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkHive.Api.Models;
using WorkHive.Api.Models.Validators;
using WorkHive.Data.Models;
using WorkHive.Data.Services;

namespace WorkHive.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ApiControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AccountController(
            WorkHiveDbContext dbContext,
            IConfiguration configuration,
            IUserService userService
        ) : base(dbContext)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        [Route("Authenticate")]
        public IActionResult Authenticate(AuthenticationModel model)
        {
            var result = _userService.Authenticate(model.EmailAddress, model.Password);
            if (!result.IsSuccessful)
            {
                return ProcessMethodResult(result);
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Email, model.EmailAddress)
            };

            var token = GetToken(claims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegistrationModel model)
        {
            try
            {
                var validation = new RegistrationValidator().Validate(model);

                if (!validation.IsValid)
                {
                    return ProcessValidationResult(validation);
                }

                return ProcessMethodResult(_userService.Register(model.Name, model.EmailAddress, model.Password));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
