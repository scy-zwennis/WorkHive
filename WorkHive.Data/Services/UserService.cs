using WorkHive.Common.ErrorCodes;
using WorkHive.Common.Extensions;
using WorkHive.Common.HelperClasses;
using WorkHive.Data.Models;
using WorkHive.Data.Models.Entities;

namespace WorkHive.Data.Services
{
    public interface IUserService
    {
        MethodResult<AuthenticationErrorCode> Authenticate(string emailAddress, string password);
        MethodResult<RegistrationErrorCodes> Register(string name, string email, string password);
    }

    public class UserService
        : IUserService
    {
        private readonly WorkHiveDbContext _dbContext;

        public UserService(WorkHiveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MethodResult<AuthenticationErrorCode> Authenticate(string email, string password)
        {

            try
            {
                var user = _dbContext.Users.SingleOrDefault(u => u.EmailAddress == email);
                if (user == null)
                {
                    return new(AuthenticationErrorCode.AccountNotFound);
                }

                if (!AuthenticationHelper.VerifyPassword(password, user.PasswordHash))
                {
                    return new(AuthenticationErrorCode.PasswordInvalid);
                }

                return MethodResult<AuthenticationErrorCode>.Success();
            }
            catch (Exception)
            {
                return new(AuthenticationErrorCode.AuthenticationFailed);
            }
        }

        public MethodResult<RegistrationErrorCodes> Register(string name, string emailAddress, string password)
        {
            try
            {
                if (_dbContext.Users.Any(u => u.EmailAddress == emailAddress.Standardize()))
                {
                    return new(RegistrationErrorCodes.AccountAlreadyExists);
                }

                var user = new User
                {
                    Name = name,
                    EmailAddress = emailAddress,
                    PasswordHash = AuthenticationHelper.GeneratePasswordHash(password)
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return new();
            }
            catch (Exception)
            {
                return new(RegistrationErrorCodes.RegistrationFailed);
            }
        }
    }
}
