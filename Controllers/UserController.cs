using chat_be.Models.Requests;
using chat_be.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace chat_be.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        // get friends
        [HttpGet("friends")]
        public async Task<IActionResult> GetFriends()
        {
            return Ok(new List<UserResponse>());
        }

        // add friend
        [HttpPost("add-friend")]
        public async Task<IActionResult> AddFriend(AddFriendRequest request)
        {
            return Ok();
        }

        // confirm friend
        [HttpPost("confirm-friend")]
        public async Task<IActionResult> ConfirmFriend(AddFriendRequest request)
        {
            return Ok();
        }
    }
}