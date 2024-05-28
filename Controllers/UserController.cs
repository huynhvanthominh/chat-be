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
        public async Task<IActionResult> GetFriends(
            [FromQuery] PaginateRequest options
        )
        {
            try
            {
                var friends = await _userService.GetFriends(options);
                var current = await _authService.CurrentUser();
                return Ok(friends.ToFriendResponse(current));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // search friends
        [HttpGet("search-friends")]
        public async Task<IActionResult> SearchFriends(
            [FromQuery] PaginateRequest options
        )
        {
            try
            {
                var users = await _userService.SearchUsers(options);
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // get friend requests
        [HttpGet("make-friend-requests")]
        public async Task<IActionResult> GetMakeFriendRequests(
             [FromQuery] PaginateRequest options
        )
        {
            try
            {
                var makeFriendRequests = await _userService.GetMakeFriendRequests(options);
                return Ok(makeFriendRequests.ToResponse());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // get received friend requests
        [HttpGet("received-make-friend-requests")]
        public async Task<IActionResult> GetReceivedFriendRequests(
            [FromQuery] PaginateRequest options
        )
        {
            try
            {
                var receivedFriendRequests = await _userService.GetReceivedFriendRequests(options);
                return Ok(receivedFriendRequests.ToResponse());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // add friend
        [HttpPost("add-friend")]
        public async Task<IActionResult> AddFriend(AddFriendRequest request)
        {
            try
            {
                await _userService.AddFriend(request);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // confirm friend
        [HttpPost("confirm-friend")]
        public async Task<IActionResult> ConfirmFriend(ConfirmFriendRequest request)
        {
            try
            {
                await _userService.ConfirmFriend(request);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}