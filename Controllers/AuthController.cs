using System.Security.Claims;
using chat_be.Mappers.Abstracts;
using chat_be.Models.Requests;
using chat_be.Models.Responses;
using chat_be.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chat_be.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("me")]
        /// <summary>
        /// Me
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the user</response>
        /// <response code="401">Unauthorized</response>
        public async Task<PayloadResponse<UserResponse>> Me()
        {
            try
            {
                var user = await _authService.CurrentUser();
                _logger.LogInformation("User: {0}", user.Username);
                throw new UnauthorizedAccessException("Unauthorized");
                // return new PayloadResponse<UserResponse>("User retrieved", true, uddser);
            }
            catch (System.Exception e)
            {
                return new PayloadResponse<UserResponse>(e.Message, false, null, 400);
            }
        }
        [HttpPost("register")]
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PayloadResponse<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _authService.Register(request);
                return new PayloadResponse<RegisterResponse>("User created", true, new(result.Username));
            }
            catch (Exception e)
            {
                return new PayloadResponse<RegisterResponse>(e.Message, false, null, 400);
            }
        }

        [HttpPost("login")]
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PayloadResponse<LoginResponse>> Login(LoginRequest request)
        {
            try
            {
                var result = await _authService.Login(request);
                return new PayloadResponse<LoginResponse>("Login successful", true, result);
            }
            catch (Exception e)
            {
                return new PayloadResponse<LoginResponse>(e.Message, false, null, 400);
            }
        }
    }
}