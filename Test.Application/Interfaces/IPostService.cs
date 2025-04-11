using TestApi.Domain.Models.Paginations;
using TestApi.Domain.Models.Posts;
using TestApi.Infrastructure.Models;

namespace TestApi.Application.Interfaces
{
    public interface IPostService
    {
        Task<ResponseModel<PostModel>> AddPost(AddPostModel addPostModel, CancellationToken cancellationToken = default);
        Task<ResponseModel<PostModel>> GetPostByIdAsync(int id, CancellationToken cancellationToken);
        Task<ResponseModel<PaginatedModel<List<PostModel>>>> GetAllPostsPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<ResponseModel<bool>> DeletePostAsync(int id, CancellationToken cancellationToken);
        Task<ResponseModel<List<PostModel>>> GetPostsByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<ResponseModel<PostModel>> UpdatePostAsync(int id, UpdatePostModel updatedModel, CancellationToken cancellationToken);
    }
}
