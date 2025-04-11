using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestApi.Application.Extensions;
using TestApi.Application.Interfaces;
using TestApi.Domain.Entities.Posts;
using TestApi.Domain.Models.Paginations;
using TestApi.Domain.Models.Posts;
using TestApi.Infrastructure.Helpers;
using TestApi.Infrastructure.Models;
using TestApi.Infrastructure.Repositories;

namespace TestApi.Application.Services
{
    public class PostService : IPostService
    {
        public IBaseRepository<Post> _postRepo;
        public AppSettings _appSettings;
        public IHttpContextAccessor _httpContextAccessor;
        public ILogger<PostService> _logger;
        public IMapper _mapper;

        public PostService(
            IBaseRepository<Post> postRepo,
            IOptions<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ILogger<PostService> logger)
        {
            _postRepo = postRepo ?? throw new ArgumentNullException(nameof(postRepo));
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResponseModel<PostModel>> AddPost(
            AddPostModel addPostModel,
            CancellationToken cancellationToken = default)
        {
            var responseModel = new ResponseModel<PostModel>();

            try
            {
                var userId = UserHelper.GetUserId(_httpContextAccessor);

                if (userId == 0)
                {
                    responseModel.UnAuthorized("User not authorized!");
                    return responseModel;
                }

                var post = _mapper.Map<Post>(addPostModel);
                post.UserId = userId;

                var addResult = await _postRepo.AddAsync(post, cancellationToken);

                if (addResult)
                {
                    var postModel = _mapper.Map<PostModel>(post);
                    responseModel.Ok(postModel);
                }
                else
                    responseModel.InternalServerError("Post could not be created at the moment. Please try again!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return responseModel;
        }

        public async Task<ResponseModel<PostModel>> GetPostByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var responseModel = new ResponseModel<PostModel>();

            try
            {
                var post = await _postRepo.GetByIdAsync(id, cancellationToken);

                if (post == null)
                {
                    responseModel.NotFound();
                    return responseModel;
                }

                responseModel.Ok(_mapper.Map<PostModel>(post));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("Post could not be retreived at the moment. Please try again!");
            }

            return responseModel;
        }

        public async Task<ResponseModel<PaginatedModel<List<PostModel>>>> GetAllPostsPaginatedAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var responseModel = new ResponseModel<PaginatedModel<List<PostModel>>>();

            try
            {
                var posts = await _postRepo.GetAllPaginatedAsync(page, pageSize, cancellationToken);
                var mappedPosts = _mapper.Map<List<PostModel>>(posts.Model);
                responseModel.Ok(posts.ToPaginatedListResponseModel<Post, PostModel>(mappedPosts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("Posts could not be retreived at the moment. Please try again!");
            }

            return responseModel;
        }

        public async Task<ResponseModel<bool>> DeletePostAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var responseModel = new ResponseModel<bool>();

            try
            {
                var post = await _postRepo.GetByIdAsync(id, cancellationToken);

                if (post == null)
                {
                    responseModel.NotFound();
                    return responseModel;
                }

                var userId = UserHelper.GetUserId(_httpContextAccessor);
                if (post.UserId != userId)
                {
                    responseModel.UnAuthorized("User is not authorized to delete this post.");
                    return responseModel;
                }

                var deleteResult = _postRepo.SoftRemove(post);
                responseModel.Ok(deleteResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("Post could not be deleted at the moment. Please try again!");
            }

            return responseModel;
        }

        public async Task<ResponseModel<List<PostModel>>> GetPostsByUserIdAsync(
            int userId,
            CancellationToken cancellationToken)
        {
            var responseModel = new ResponseModel<List<PostModel>>();

            try
            {
                var currentUserId = UserHelper.GetUserId(_httpContextAccessor);
                if (currentUserId != userId)
                {
                    responseModel.UnAuthorized("User is not authorized to read these posts.");
                    return responseModel;
                }

                var posts = await _postRepo.GetByConditionAsync(p => p.UserId == userId, cancellationToken);
                var mapped = _mapper.Map<List<PostModel>>(posts);
                responseModel.Ok(mapped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("User posts could not be retreived at the moment. Please try again!");
            }

            return responseModel;
        }

        public async Task<ResponseModel<PostModel>> UpdatePostAsync(
            int id,
            UpdatePostModel updatedModel,
            CancellationToken cancellationToken)
        {
            var responseModel = new ResponseModel<PostModel>();

            try
            {
                var post = await _postRepo.GetByIdAsync(id, cancellationToken);

                if (post == null)
                {
                    responseModel.NotFound();
                    return responseModel;
                }

                var userId = UserHelper.GetUserId(_httpContextAccessor);
                if (post.UserId != userId)
                {
                    responseModel.UnAuthorized("User is not authorized to update this post.");
                    return responseModel;
                }

                _mapper.Map(updatedModel, post);

                var updateResult = _postRepo.Update(post);
                responseModel.Ok(_mapper.Map<PostModel>(post));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("Failed to update post.");
            }

            return responseModel;
        }
    }
}
