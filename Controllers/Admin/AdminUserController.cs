using chat_be.Mappers.Abstracts;
using chat_be.Models;
using chat_be.Models.Responses;
using chat_be.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace chat_be.Controllers.Admin
{

    [Route("api/admin/user")]
    [ApiController]
    public class AdminUserController : Controller
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IMapper _mapper;

        public AdminUserController(
            IAdminUserService adminUserService,
            IMapper mapper
            )
        {
            _adminUserService = adminUserService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<PayloadResponse<PaginatedResponse<UserResponse>>> GetUsers()
        {
            try
            {
                var users = await _adminUserService.GetUsers();
                return new PayloadResponse<PaginatedResponse<UserResponse>>(
                    "Users retrieved",
                    true,
                    new PaginatedResponse<UserResponse>(
                        1,
                        1,
                        1,
                        users.Count,
                        users.ToResponse()
                        ),
                    200
                    );
            }
            catch (Exception e)
            {
                return new PayloadResponse<PaginatedResponse<UserResponse>>(e.Message, false, null);
            }
        }
    }
}