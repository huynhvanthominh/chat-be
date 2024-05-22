using chat_be.Models;
using chat_be.Models.Requests;
using chat_be.Models.Responses;
using chat_be.Services;
using Microsoft.AspNetCore.Mvc;

namespace chat_be.Controllers
{

    [Route("api/messages")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("")]
        // get messages
        /// <summary>
        /// Get messages
        /// </summary>
        /// <returns></returns>
        public async Task<PayloadResponse<PaginatedResponse<MessageGroupModel>>> GetMessages([FromQuery] PaginateRequest options)
        {
            try
            {
                var res = await _messageService.GetMessages(options);
                return new PayloadResponse<PaginatedResponse<MessageGroupModel>>()
                {
                    Message = "Get messages successfully",
                    StatusCode = 200,
                    Success = true,
                    Payload = res
                };
            }
            catch (Exception e)
            {
                return new PayloadResponse<PaginatedResponse<MessageGroupModel>>()
                {
                    Message = e.Message,
                    StatusCode = 500,
                    Success = false,
                    Payload = null
                };
            }
        }

        // get messages by specific user
        /// <summary>
        /// Get messages by specific user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("get-messages/{messageGroupId}")]
        public async Task<IActionResult> GetMessages(int messageGroupId, [FromQuery] PaginateRequest options)
        {
            var res = await _messageService.GetMessage(messageGroupId, options);
            return Ok(res);
        }

        // send message
        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="username"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("send-message/")]
        public async Task<IActionResult> SendMessage(SendMessageRequest request)
        {
            await _messageService.SendMessage(request);
            return Ok();
        }
    }

}