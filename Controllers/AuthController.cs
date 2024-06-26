using chat_be.Models;
using chat_be.Models.Requests;
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

        // private readonly IMapper _mapper;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger
            // IMapper mapper
            )
        {
            _authService = authService;
            _logger = logger;
            // _mapper = mapper;
        }

        [Authorize]
        [HttpGet("me")]
        /// <summary>
        /// Me
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns the user</response>
        /// <response code="401">Unauthorized</response>
        public async Task<IActionResult> Me()
        {
            try
            {
                var user = await _authService.CurrentUser();
                return Ok(user.ToResponse());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _authService.Register(request);
                return Ok(result.ToResponse());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _authService.Login(request);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPatch("update")]
        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">Returns the user</response>
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequest request)
        {
            try
            {
                var result = await _authService.UpdateProfile(request);
                return Ok(result.ToResponse());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}