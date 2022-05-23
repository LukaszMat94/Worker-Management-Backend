using WorkerManagementAPI.Data.Models.UserDtos;

namespace WorkerManagementAPI.Services.UserService.Service
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<Dictionary<string, string>> LoginUserAsync(LoginUserDto loginUserDto);

        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(long id);
        Task<UserDto> UpdateUserAsync(UpdateUserDto updateUserDto);
        Task DeleteUserAsync(long id);
        Task<UpdateUserTechnologyDto> AssignTechnologyToUserAsync(PatchUserTechnologyDto patchUserTechnologyDto);
        Task UnassignTechnologyFromUserAsync(PatchUserTechnologyDto patchUserTechnologyDto);
    }
}
