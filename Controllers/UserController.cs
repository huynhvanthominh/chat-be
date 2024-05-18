using chat_be.Mappers.Abstracts;
using chat_be.Models.Requests;
using chat_be.Models.Responses;
using chat_be.Services;
using chat_be.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chat_be.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger,
            IMapper mapper
            )
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        // get friends
        [HttpGet("friends")]
        public async Task<IActionResult> GetFriends()
        {
            return Ok(new List<UserResponse>());
        }

        // get friend requests
        [HttpGet("make-friend-requests")]
        public async Task<PayloadResponse<PaginatedResponse<UserResponse>>> GetMakeFriendRequests(
             [FromQuery] PaginateRequest options
        )
        {
            _logger.LogInformation("Get make friend requests: ");
            _logger.LogInformation(options.CountPerPage.ToString());
            _logger.LogInformation(options.Page.ToString());
            try
            {
                var makeFriendRequests = await _userService.GetMakeFriendRequests(options);
                return new PayloadResponse<PaginatedResponse<UserResponse>>(
                    "Friend requests retrieved",
                    true,
                    makeFriendRequests.ToResponse(),
          200);
            }
            catch (Exception e)
            {
                return new PayloadResponse<PaginatedResponse<UserResponse>>(e.Message, false, null, 400);
            }
        }

        // get received friend requests
        [HttpGet("received-make-friend-requests")]
        public async Task<IActionResult> GetReceivedFriendRequests()
        {
            throw new NotImplementedException();
        }

        // add friend
        [HttpPost("add-friend")]
        public async Task<PayloadResponse<Boolean>> AddFriend(AddFriendRequest request)
        {
            _logger.LogInformation("Add friend request: ");
            try
            {
                await _userService.AddFriend(request);
                return new PayloadResponse<Boolean>("Friend request sent", true, true);
            }
            catch (Exception e)
            {
                return new PayloadResponse<Boolean>(e.Message, false, false, 400);
            }
        }

        // confirm friend
        [HttpPost("confirm-friend")]
        public async Task<IActionResult> ConfirmFriend(AddFriendRequest request)
        {
            return Ok();
        }
    }
}