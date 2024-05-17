using chat_be.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace chat_be.Controllers{

    [Route("api/message")]
    [ApiController]
    public class MessageController: Controller{
        
        // get messages by specific user
        /// <summary>
        /// Get messages by specific user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("get-messages/{username}")]
        public async Task<IActionResult> GetMessages(string username){
            return Ok();
        }
    
        // send message
        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="username"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("send-message/{username}")]
        public async Task<IActionResult> SendMessage(string username, SendMessageRequest request){
            return Ok();
        }
    }
}