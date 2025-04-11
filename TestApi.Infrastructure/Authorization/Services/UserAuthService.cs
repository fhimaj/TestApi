using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TestApi.Domain.Entities.Users;
using TestApi.Domain.Models;
using TestApi.Domain.Models.Users;
using TestApi.Infrastructure.Authorization.Interfaces;
using TestApi.Infrastructure.Extensions;
using TestApi.Infrastructure.Helpers;
using TestApi.Infrastructure.Models;
using TestApi.Infrastructure.Repositories;

namespace TestApi.Infrastructure.Authorization.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IBaseRepository<User> _userRepo;
        private AppSettings _appSettings;
        private ILogger<UserAuthService> _logger;

        public UserAuthService(
            IBaseRepository<User> userRepo,
            IOptions<AppSettings> appSettings,
            ILogger<UserAuthService> logger)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task<ResponseModel<UserCredentials>> GenerateCredentials(
            AddUserModel addUserModel,
            CancellationToken cancellationToken = default)
        {
            var responseModel = new ResponseModel<UserCredentials>();

            var passwordHash = CreateHashPassword(addUserModel.Password);

            if (passwordHash.IsEmpty())
                responseModel.InternalServerError();
            else
                responseModel.Ok(new UserCredentials { PasswordHash = passwordHash });

            return responseModel;
        }

        public async Task<ResponseModel<UserLoginResponseModel>> LoginAsync(
            User user,
            string password,
            CancellationToken cancellationToken = default)
        {
            var responseModel = new ResponseModel<UserLoginResponseModel>();

            try
            {
                if (!VerifyPassword(password, user.PasswordHash))
                {
                    responseModel.BadRequest("Invalid credentials.");
                    return responseModel;
                }

                var token = GenerateToken(user);

                if (token.IsEmpty())
                {
                    responseModel.InternalServerError("Could not login user. Please try again.");
                    return responseModel;
                }

                var loginResponse = new UserLoginResponseModel
                {
                    Token = token
                };

                responseModel.Ok(loginResponse);
                return responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                responseModel.InternalServerError("Could not login user. Please try again.");
                return responseModel;
            }
        }

        private string CreateHashPassword(string password)
        {
            try
            {
                byte[] salt = new byte[128 / 8];
                using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                return Convert.ToBase64String(salt) + ":" + hashed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return string.Empty;
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            try
            {
                var parts = storedHash.Split(':');
                if (parts.Length != 2) return false;

                byte[] salt = Convert.FromBase64String(parts[0]);
                var storedPasswordHash = parts[1];

                var hashedEnteredPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: enteredPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                return storedPasswordHash == hashedEnteredPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        private string GenerateToken(User user)
        {
            try
            {
                var claims = UserHelper.CreateUserClaims(user);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtConfig.SecretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _appSettings.JwtConfig.Issuer,
                    audience: _appSettings.JwtConfig.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return string.Empty;
            }
        }
    }
}
