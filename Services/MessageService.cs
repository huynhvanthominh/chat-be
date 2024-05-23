using System.Text.Json;
using chat_be.Data;
using chat_be.Models;
using chat_be.Models.Requests;
using chat_be.Models.Responses;
using chat_be.Services.Abstracts;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace chat_be.Services
{
    public class MessageService : IMessageService
    {
        private readonly DatabaseContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<MessageService> _logger;
        private readonly IUserService _userService;

        public MessageService(
            DatabaseContext context,
            IAuthService authService,
            IUserService userService,
            ILogger<MessageService> logger
            )
        {
            _context = context;
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<PaginatedResponse<MessageResponse>> GetMessage(int messageGroupId, PaginateRequest options)
        {
            var message = await _context.MessageModels
            .Select(m => new MessageResponse
            {
                Id = m.Id,
                Message = m.Message,
                UserId = m.UserId,
                CreatedAt = m.CreatedAt,
                MessageGroupId = m.MessageGroupId
            })
            .OrderByDescending(m => m.CreatedAt)
            .ToPaginatedListAsync(options.Page, options.CountPerPage);
            return message;
        }

        public async Task<MessageGroupModel?> GetMessageGroup(int id)
        {
            return await _context.MessageGroupModels.FindAsync(id);
        }

        public async Task<PaginatedResponse<MessageGroupModel>> GetMessages(PaginateRequest options)
        {
            var currentUser = await _authService.CurrentUser();
            var messageGroups = await _context.MessageGroupUserModels
            .Where(mgu => mgu.UserId == currentUser.Id)
            .GroupJoin(
                    _context.MessageGroupModels,
                    mgu => mgu.MessageGroup,
                    mg => mg,
                    (mgu, mg) => new MessageGroupModel
                    {
                        Id = mgu.MessageGroupId,
                        Name = string.IsNullOrEmpty(mg.FirstOrDefault().Name)
                        ? mg.FirstOrDefault().MessageGroupUsers.Where(x => x.UserId != currentUser.Id).FirstOrDefault().User.DisplayName
                        : mg.FirstOrDefault().Name,
                        MessageGroupUsers = mg.FirstOrDefault().MessageGroupUsers,
                    }
                )
            .ToPaginatedListAsync(options.Page, options.CountPerPage);
            return messageGroups;
        }

        public async Task<MessageResponse> SendMessage(SendMessageRequest content)
        {
            var currentUser = await _authService.CurrentUser();
            var messageGroup = await _context.MessageGroupModels.GroupJoin(
                    _context.MessageGroupUserModels,
                    mg => mg.Id,
                    mgu => mgu.MessageGroupId,
                    (mg, mgu) => new MessageGroupModel
                    {
                        Id = mg.Id,
                        Name = mg.Name,
                        MessageGroupUsers = mgu.ToList()
                    }
                )
                .FirstOrDefaultAsync(mg => mg.Id == content.MessageGroupId
            );
            if (messageGroup == null)
            {
                throw new Exception("Message group not found");
            }
            var userInGroup = messageGroup.MessageGroupUsers.Where(x => x.UserId == currentUser.Id).FirstOrDefault() ?? throw new Exception("User not in group");
            var message = new MessageModel
            {
                Message = content.Message,
                MessageGroupId = messageGroup.Id,
                UserId = currentUser.Id
            };
            await _context.MessageModels.AddAsync(message);
            await _context.SaveChangesAsync();
            return new MessageResponse{
                Id = message.Id,
                Message = message.Message,
                UserId = message.UserId,
                CreatedAt = message.CreatedAt,
                MessageGroupId = message.MessageGroupId,
                UserIds = messageGroup.MessageGroupUsers.Select(x => x.UserId).ToList()
            };
        }
    }
}