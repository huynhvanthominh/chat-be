using chat_be.Models.Requests;
using chat_be.Services;
using chat_be.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;

namespace chat_be.Hubs
{
    [Authorize]
    public class ChatHup : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        public ChatHup(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }
        public async Task SendMessage(SendMessageRequest request)
        {
            if (string.IsNullOrEmpty(request.Message))
            {
                return;
            }
            var ms = await _messageService.SendMessage(request);
            await Clients.Users(ms.UserIds.Select(x => x.ToString())).SendAsync("ReceiveMessage", ms);
        }
        public async Task SendRequestAddFriend(AddFriendRequest request)
        {
            try
            {
                var makeFriend = await _userService.AddFriend(request);
                await Clients.User(makeFriend.UserId.ToString()).SendAsync("SendRequestAddFriend", makeFriend);
                await Clients.User(makeFriend.FriendId.ToString()).SendAsync("ReceiveRequestAddFriend", makeFriend);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveSystemMessage",
                                        $"{Context.UserIdentifier} joined.");
            await base.OnConnectedAsync();
        }
    }
}
