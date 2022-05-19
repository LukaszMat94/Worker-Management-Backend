using Microsoft.AspNetCore.Mvc;
using WorkerManagementAPI.Data.Models.UserDtos;
using WorkerManagementAPI.Services.UserService.Service;

namespace WorkerManagementAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService =userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            UserDto userDto = await _userService.RegisterUserAsync(registerUserDto);
            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            string token = await _userService.LoginUserAsync(loginUserDto);
            return Ok(token);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            List<UserDto> usersDto = await _userService.GetAllUsersAsync();
            return Ok(usersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] long id)
        {
            UserDto userDto = await _userService.GetUserByIdAsync(id);
            return Ok(userDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            UserDto userDto = await _userService.UpdateUserAsync(updateUserDto);
            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] long id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpPatch("assignTechnology")]
        public async Task<IActionResult> AssignTechnologyToUser([FromBody] PatchUserTechnologyDto patchUserTechnologyDto)
        {
            UpdateUserTechnologyDto updateUserTechnologyDto = await _userService.AssignTechnologyToUserAsync(patchUserTechnologyDto);
            return Ok(updateUserTechnologyDto);
        }

        [HttpPatch("unassignTechnology")]
        public async Task<IActionResult> UnassignTechnologyFromUser([FromBody] PatchUserTechnologyDto patchUserTechnologyDto)
        {
            await _userService.UnassignTechnologyFromUserAsync(patchUserTechnologyDto);
            return NoContent();
        }
    }
}
