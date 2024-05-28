using chat_be.Models;
using chat_be.Models.Responses;

public static class UserModelExtensions
{
    private static IHttpContextAccessor? _httpContextAccessor;
    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public static UserResponse ToResponse(this UserModel user)
    {
        if (_httpContextAccessor?.HttpContext?.Request.Host.Value == null)
        {
            throw new Exception("HttpContextAccessor is not configured");
        }
        var http = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/";

        return new UserResponse(
            user.Id,
            user.Username,
            user.DisplayName ?? "",
            http + user.Avatar
        );
    }
    public static List<UserResponse> ToResponse(this List<UserModel> users)
    {
        return users.Select(x => x.ToResponse()).ToList();
    }
}