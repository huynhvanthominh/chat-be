using chat_be.Models;
using chat_be.Models.Requests;
using chat_be.Models.Responses;

namespace chat_be.Services{
    public interface IMessageService
    {
        Task<PaginatedResponse<MessageGroupModel>> GetMessages(PaginateRequest options);
        Task<PaginatedResponse<MessageResponse>> GetMessage(int messageGroupId, PaginateRequest options);
        Task<MessageResponse> SendMessage(SendMessageRequest content);
    }
}