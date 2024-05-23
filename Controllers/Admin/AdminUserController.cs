using chat_be.Mappers.Abstracts;
using chat_be.Models;
using chat_be.Models.Requests;
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
        // private readonly IMapper _mapper;

        public AdminUserController(
            IAdminUserService adminUserService
            // IMapper mapper
            )
        {
            _adminUserService = adminUserService;
            // _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] PaginateRequest options)
        {
            try
            {
                var users = await _adminUserService.GetUsers(options);
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}