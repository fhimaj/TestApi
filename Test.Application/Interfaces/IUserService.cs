using TestApi.Domain.Models;
using TestApi.Domain.Models.Paginations;
using TestApi.Domain.Models.Users;
using TestApi.Infrastructure.Models;

namespace TestApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<ResponseModel<UserModel>> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<ResponseModel<UserModel>> AddUserAsync(AddUserModel addUserModel, CancellationToken cancellationToken = default);
        Task<ResponseModel<UserModel>> UpdateUserAsync(int id, UpdateUserModel updatedModel, CancellationToken cancellationToken);
        Task<ResponseModel<bool>> DeleteUserAsync(int id, CancellationToken cancellationToken);
        Task<ResponseModel<PaginatedModel<List<UserModel>>>> GetAllUsersPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<ResponseModel<bool>> UpdateUserPreferencesAsync(int id, string preferences, CancellationToken cancellationToken);
        Task<ResponseModel<UserLoginResponseModel>> LoginAsync(string userName, string password, CancellationToken cancellationToken = default);
    }
}
