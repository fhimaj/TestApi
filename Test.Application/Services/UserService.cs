using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestApi.Application.Extensions;
using TestApi.Application.Interfaces;
using TestApi.Domain.Entities.Users;
using TestApi.Domain.Models;
using TestApi.Domain.Models.Paginations;
using TestApi.Domain.Models.Users;
using TestApi.Infrastructure.Authorization.Interfaces;
using TestApi.Infrastructure.Extensions;
using TestApi.Infrastructure.Helpers;
using TestApi.Infrastructure.Models;
using TestApi.Infrastructure.Repositories;

namespace TestApi.Application.Services
{
    public class UserService : IUserService
    {
        public IBaseRepository<User> _userRepo;
        public AppSettings _appSettings;
        public IHttpContextAccessor _httpContextAccessor;
        public ILogger<UserService> _logger;
        public IMapper _mapper;
        public IUserAuthService _userAuthService;

        public UserService(
            IBaseRepository<User> userRepo,
            IOptions<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IUserAuthService userAuthService,
            ILogger<UserService> logger)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userAuthService = userAuthService ?? throw new ArgumentNullException(nameof(userAuthService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResponseModel<UserModel>> AddUserAsync(
            AddUserModel addUserModel,
            CancellationToken cancellationToken = default)
        {
            var responseModel = new ResponseModel<UserModel>();

            try
            {
                var existingUser = (await _userRepo.GetByConditionAsync(u => u.Email == addUserModel.Email)).FirstOrDefault();
                if (existingUser != null)
                {
                    responseModel.BadRequest("User with given email already exists.");
                    return responseModel;
                }

                var credentials = await _userAuthService.GenerateCredentials(addUserModel, cancellationToken);

                if (!credentials.IsSuccessWithResult())
                {
                    responseModel.InternalServerError("User could not be added. Please try again!");
                    return responseModel;
                }

                var user = _mapper.Map<User>(addUserModel);
                SetTemporaryUserId(user.Id.ToString());
                user.PasswordHash = credentials?.Result?.PasswordHash ?? string.Empty;
                var addResult = await _userRepo.AddAsync(user, cancellationToken);

                if (!addResult)
                {
                    responseModel.InternalServerError("User could not be added. Please try again!");
                    return responseModel;
                }

                responseModel.Ok(_mapper.Map<UserModel>(user));
                return responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("An error occurred while adding the user.");
            }

            return responseModel;
        }

        public async Task<ResponseModel<UserModel>> GetUserByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var responseModel = new ResponseModel<UserModel>();

            try
            {
                var user = await _userRepo.GetByIdAsync(id, cancellationToken);

                if (user == null)
                {
                    responseModel.NotFound();
                    return responseModel;
                }

                responseModel.Ok(_mapper.Map<UserModel>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("Failed to retrieve user.");
            }

            return responseModel;
        }


        public async Task<ResponseModel<UserModel>> UpdateUserAsync(
            int id,
            UpdateUserModel updatedModel,
            CancellationToken cancellationToken)
        {
            var responseModel = new ResponseModel<UserModel>();

            try
            {
                var user = await _userRepo.GetByIdAsync(id, cancellationToken);

                if (user == null)
                {
                    responseModel.NotFound();
                    return responseModel;
                }

                var userId = UserHelper.GetUserId(_httpContextAccessor);
                if (user.Id != userId)
                {
                    responseModel.UnAuthorized("User is not authorized.");
                    return responseModel;
                }

                _mapper.Map(updatedModel, user);

                var result = _userRepo.Update(user);

                if (!result)
                {
                    responseModel.InternalServerError("User could not be updated.");
                    return responseModel;
                }

                var userModel = _mapper.Map<UserModel>(user);
                responseModel.Ok(userModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("Failed to update user.");
            }

            return responseModel;
        }

        public async Task<ResponseModel<bool>> UpdateUserPreferencesAsync(
            int id,
            string preferences,
            CancellationToken cancellationToken)
        {
            var responseModel = new ResponseModel<bool>();

            try
            {
                var user = await _userRepo.GetByIdAsync(id, cancellationToken);

                if (user == null)
                {
                    responseModel.NotFound();
                    return responseModel;
                }

                var userId = UserHelper.GetUserId(_httpContextAccessor);
                if (user.Id != userId)
                {
                    responseModel.UnAuthorized("User is not authorized.");
                    return responseModel;
                }

                if (!preferences.IsValidJson())
                {
                    responseModel.BadRequest("Preferences must be a valid Json.");
                    return responseModel;
                }

                user.Preferences = preferences;

                var updateResult = _userRepo.Update(user);
                if (!updateResult)
                {
                    responseModel.InternalServerError("Failed to update preferences. Please try again!");
                    return responseModel;
                }

                responseModel.Ok(updateResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("Failed to update preferences. Please try again!");
            }

            return responseModel;
        }

        public async Task<ResponseModel<PaginatedModel<List<UserModel>>>> GetAllUsersPaginatedAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var responseModel = new ResponseModel<PaginatedModel<List<UserModel>>>();

            try
            {
                var users = await _userRepo.GetAllPaginatedAsync(page, pageSize, cancellationToken);
                var mappedUsers = _mapper.Map<List<UserModel>>(users.Model);
                responseModel.Ok(users.ToPaginatedListResponseModel<User, UserModel>(mappedUsers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("Failed to retrieve users.");
            }

            return responseModel;
        }


        public async Task<ResponseModel<bool>> DeleteUserAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var responseModel = new ResponseModel<bool>();

            try
            {
                var user = await _userRepo.GetByIdAsync(id, cancellationToken);

                if (user == null)
                {
                    responseModel.NotFound();
                    return responseModel;
                }

                var result = _userRepo.SoftRemove(user);

                if (!result)
                    responseModel.InternalServerError("Could not remove user. Please try again.");
                else
                    responseModel.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("Failed to delete user.");
            }

            return responseModel;
        }

        private void SetTemporaryUserId(string userId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Items["SignUpUserId"] = "SignUpId:" + userId;
            }
        }

        public async Task<ResponseModel<UserLoginResponseModel>> LoginAsync(string userName, string password, CancellationToken cancellationToken = default)
        {
            var responseModel = new ResponseModel<UserLoginResponseModel>();

            try
            {
                var existingUser = (await _userRepo.GetByConditionAsync(u => u.UserName == userName)).FirstOrDefault();
                if (existingUser == null)
                {
                    responseModel.BadRequest("User with given username does not exist.");
                    return responseModel;
                }

                var loginResponse = await _userAuthService.LoginAsync(existingUser, password, cancellationToken);

                if (!loginResponse.IsSuccessWithResult())
                {
                    responseModel.InternalServerError("User could not be logged in. Please try again!");
                    return responseModel;
                }

                return loginResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                responseModel.InternalServerError("An error occurred while singing in the user.");
            }

            return responseModel;
        }
    }
}
