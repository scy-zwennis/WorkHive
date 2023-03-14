using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WorkHive.Api.Models;
using WorkHive.Api.Models.Validators;
using WorkHive.Data.Models;
using WorkHive.Data.Services;

namespace WorkHive.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        private readonly IUserService UserService;

        public AccountController(
            WorkHiveDbContext dbContext,
            IUserService userService
        ) : base(dbContext)
        {
            UserService = userService;
        }

        [HttpPost]
        [Route("Authenticate")]
        public IActionResult Authenticate(AuthenticationModel model)
        {
            return ProcessMethodResult(UserService.Authenticate(model.EmailAddress, model.Password));
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

                return ProcessMethodResult(UserService.Register(model.Name, model.EmailAddress, model.Password));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
