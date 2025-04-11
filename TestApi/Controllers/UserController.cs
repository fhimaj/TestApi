using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestApi.Application.Interfaces;
using TestApi.Domain.Models.Paginations;
using TestApi.Domain.Models.Users;
using TestApi.Infrastructure.Models;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel<UserModel>>> Login(
           [FromQuery] string userName,
           [FromQuery] string password,
           CancellationToken cancellationToken)
        {
            var result = await _userService.LoginAsync(
                userName,
                password,
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel<UserModel>>> AddUser(
            [FromBody] AddUserModel model,
            CancellationToken cancellationToken)
        {
            var result = await _userService.AddUserAsync(
                model,
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<UserModel>>> GetUserById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _userService.GetUserByIdAsync(
                id,
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<UserModel>>> UpdateUser(
            int id,
            [FromBody] UpdateUserModel model,
            CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateUserAsync(
                id,
                model,
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/preferences")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateUserPreferences(
            int id,
            [FromBody] string preferences,
            CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateUserPreferencesAsync(
                id,
                preferences,
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<PaginatedModel<List<UserModel>>>>> GetAllPaginated(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllUsersPaginatedAsync(
                page,
                pageSize,
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<bool>>> DeleteUser(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _userService.DeleteUserAsync(
                id,
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }
    }
}
