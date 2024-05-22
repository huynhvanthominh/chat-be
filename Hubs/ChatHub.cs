using chat_be.Models.Requests;
using chat_be.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace chat_be.Hubs
{
    [Authorize]
    public class ChatHup : Hub
    {
        private readonly IMessageService _messageService;
        public ChatHup(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public async Task SendMessage(string group, string message)
        {
            var ms = await _messageService.SendMessage(new SendMessageRequest
            {
                Message = message,
                MessageGroupId = int.Parse(group)
            });
            await Clients.All.SendAsync("ReceiveMessage", group, ms);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveSystemMessage",
                                        $"{Context.UserIdentifier} joined.");
            await base.OnConnectedAsync();
        }
    }
}