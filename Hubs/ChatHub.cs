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
        public async Task SendMessage(SendMessageRequest request)
        {
            if(string.IsNullOrEmpty(request.Message))
            {
                return;
            }
            var ms = await _messageService.SendMessage(request);
            await Clients.Users(ms.UserIds.Select(x => x.ToString())).SendAsync("ReceiveMessage", ms);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveSystemMessage",
                                        $"{Context.UserIdentifier} joined.");
            await base.OnConnectedAsync();
        }
    }
}
