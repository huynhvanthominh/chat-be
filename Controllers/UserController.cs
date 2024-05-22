using chat_be.Extensions;
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
        private readonly IAuthService _authService;
        // private readonly IMapper _mapper;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger,
            IAuthService authService
            // IMapper mapper
            )
        {
            _userService = userService;
            _logger = logger;
            _authService = authService;
            // _mapper = mapper;
        }

        // get friends
        [HttpGet("friends")]
        public async Task<PayloadResponse<PaginatedResponse<FriendResponse>>> GetFriends(
            [FromQuery] PaginateRequest options
        )
        {
            try
            {
                var friends = await _userService.GetFriends(options);
                var current = await _authService.CurrentUser();
                return new PayloadResponse<PaginatedResponse<FriendResponse>>(
                    "Friends retrieved",
                    true,
                    friends.ToFriendResponse(current),
                    200);
            }
            catch (Exception e)
            {
                return new PayloadResponse<PaginatedResponse<FriendResponse>>(e.Message, false, null, 400);
            }
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
        public async Task<PayloadResponse<PaginatedResponse<UserResponse>>> GetReceivedFriendRequests(
            [FromQuery] PaginateRequest options
        )
        {
            try
            {
                var receivedFriendRequests = await _userService.GetReceivedFriendRequests(options);
                return new PayloadResponse<PaginatedResponse<UserResponse>>(
                    "Received friend requests retrieved",
                    true,
                    receivedFriendRequests.ToResponse(),
                    200);
            }
            catch (Exception e)
            {
                return new PayloadResponse<PaginatedResponse<UserResponse>>(e.Message, false, null, 400);
            }
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
        public async Task<PayloadResponse<Boolean>> ConfirmFriend(ConfirmFriendRequest request)
        {
            try
            {
                await _userService.ConfirmFriend(request);
                return new PayloadResponse<Boolean>("Friend request confirmed", true, true);
            }
            catch (Exception e)
            {
                return new PayloadResponse<Boolean>(e.Message, false, false, 400);
            }
        }
    }
}