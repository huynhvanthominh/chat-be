using chat_be.Models;
using chat_be.Models.Responses;

public static class IUserServiceExtensions
    {
        public static PaginatedResponse<UserResponse> ToResponse(this PaginatedResponse<UserModel> paginatedResponse)
        {
            return new PaginatedResponse<UserResponse>(
                paginatedResponse.TotalPage,
                paginatedResponse.CurrentPage,
                paginatedResponse.CountPerPage,
                paginatedResponse.TotalCount,
                paginatedResponse.Data.Select(x => x.ToResponse()).ToList()
            )!;
        }
    }