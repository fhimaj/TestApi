using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestApi.Application.Interfaces;
using TestApi.Domain.Models.Paginations;
using TestApi.Domain.Models.Posts;
using TestApi.Infrastructure.Models;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel<PostModel>>> AddPost(
            [FromBody] AddPostModel model,
            CancellationToken cancellationToken)
        {
            var result = await _postService.AddPost(model, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<PostModel>>> GetPostById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _postService.GetPostByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<ResponseModel<PaginatedModel<List<PostModel>>>>> GetAllPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _postService.GetAllPostsPaginatedAsync(page, pageSize, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<bool>>> DeletePost(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _postService.DeletePostAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ResponseModel<List<PostModel>>>> GetPostsByUser(
            int userId,
            CancellationToken cancellationToken)
        {
            var result = await _postService.GetPostsByUserIdAsync(userId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseModel<PostModel>>> UpdatePost(
            int id,
            [FromBody] UpdatePostModel model,
            CancellationToken cancellationToken)
        {
            var result = await _postService.UpdatePostAsync(id, model, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
