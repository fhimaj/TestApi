using TestApi.Domain.Entities.Users;
using TestApi.Domain.Models;
using TestApi.Domain.Models.Users;
using TestApi.Infrastructure.Models;

namespace TestApi.Infrastructure.Authorization.Interfaces
{
    public interface IUserAuthService
    {
        Task<ResponseModel<UserCredentials>> GenerateCredentials(AddUserModel addUserModel, CancellationToken cancellationToken = default);
        Task<ResponseModel<UserLoginResponseModel>> LoginAsync(User user, string password, CancellationToken cancellationToken = default);
    }
}
