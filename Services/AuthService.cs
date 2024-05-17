using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using chat_be.Models;
using chat_be.Models.Requests;
using chat_be.Models.Responses;
using chat_be.Services.Abstracts;
using Microsoft.IdentityModel.Tokens;

namespace chat_be.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUserService _userService;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _config;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthService(
            IUserService userService,
            ILogger<AuthService> logger,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _userService = userService;
            _logger = logger;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserModel> Register(RegisterRequest user)
        {
            var userExists = await _userService.GetUser(user.Username);
            _logger.LogInformation("User exists: {0}", userExists?.Username);
            if (userExists != null)
            {
                throw new Exception("User already exists");
            }

            if (user.Password != user.ConfirmPassword)
            {
                throw new Exception("Password and Confirm Password do not match");
            }

            var newUser = new UserModel(
                user.Username,
                user.Password,
                UserRole.user,
                user.DisplayName
            );
            return await _userService.CreateUser(newUser);
        }
        public async Task<LoginResponse> Login(LoginRequest user)
        {
            var userExists = await _userService.GetUser(user.Username, user.Password);
            if (userExists != null)
            {
                return GenerateAccessToken(userExists);
            }
            else
            {
                throw new Exception("Invalid username or password");
            }
        }

        private LoginResponse GenerateAccessToken(UserModel user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            return GetToken(claims);
        }

        private LoginResponse GetToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);
            return new LoginResponse(
                new JwtSecurityTokenHandler().WriteToken(token),
                null,
                token.ValidTo
            );
        }

        public async Task<UserModel> CurrentUser()
        {
            var userName = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userName == null)
            {
                throw new Exception("User not found");
            }
            var user = await _userService.GetUser(userName);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<UserModel> UpdateProfile(UpdateProfileRequest request)
        {
            var user = await CurrentUser();
            user.DisplayName = request.DisplayName;
            return await _userService.UpdateUser(user);
        }
    }
}