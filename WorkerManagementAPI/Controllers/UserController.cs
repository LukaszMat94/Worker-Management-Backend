using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkerManagementAPI.Data.Models.RefreshTokenDtos;
using WorkerManagementAPI.Data.Models.UserDtos;
using WorkerManagementAPI.Services.UserService.Service;

namespace WorkerManagementAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService =userService;
        }

        [HttpPost("refreshTokens")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokenDto refreshTokenDto)
        {
            Dictionary<string, string> tokens = await _userService.GetRefreshedTokensAsync(refreshTokenDto);

            return Ok(tokens);
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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                UserDto userDto = await _userService.RegisterUserAsync(registerUserDto);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            Dictionary<string, string> tokens = await _userService.LoginUserAsync(loginUserDto);

            return Ok(tokens);
        }

        [HttpPost("logout/{userId}")]
        public async Task<IActionResult> Logout([FromRoute] long userId)
        {
            await _userService.LogoutUserAsync(userId);

            return NoContent();
        }
    }
}
