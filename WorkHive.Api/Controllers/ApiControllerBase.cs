using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WorkHive.Common.HelperClasses;
using WorkHive.Data.Models;

namespace WorkHive.Api.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected readonly WorkHiveDbContext _dbContext;

        public ApiControllerBase(WorkHiveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected IActionResult ProcessMethodResult<TReturn, TError>(
            MethodResult<TReturn, TError> result) where TError : Enum
        {
            if (result.IsSuccessful)
            {
                return Ok(result.Result);
            }

            return BadRequest(result.GetErrors());
        }

        protected IActionResult ProcessMethodResult<TError>(
            MethodResult<TError> result) where TError : Enum
        {
            if (result.IsSuccessful)
            {
                return Ok();
            }

            return BadRequest(result.GetErrors());
        }

        protected IActionResult ProcessValidationResult(ValidationResult result)
        {
            if (result.IsValid)
            {
                return Ok();
            }

            return BadRequest(result.Errors.Select(x => x.ErrorCode).ToList());
        }
    }
}
