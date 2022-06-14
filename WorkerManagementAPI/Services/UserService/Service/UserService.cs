using AutoMapper;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Entities.Enums;
using WorkerManagementAPI.Data.JwtToken;
using WorkerManagementAPI.Data.Models.RefreshTokenDtos;
using WorkerManagementAPI.Data.Models.UserDtos;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.ExceptionsTemplate;
using WorkerManagementAPI.Services.MailService.Service;
using WorkerManagementAPI.Services.PasswordService.Service;
using WorkerManagementAPI.Services.RoleService.Repository;
using WorkerManagementAPI.Services.TechnologyService.Repository;
using WorkerManagementAPI.Services.TokenService.Service;
using WorkerManagementAPI.Services.UserService.Repository;

namespace WorkerManagementAPI.Services.UserService.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITechnologyRepository _technologyRepository;

        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IMailService _mailService;

        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, 
            IRoleRepository roleRepository,
            ITechnologyRepository technologyRepository,
            IPasswordService passwordService,
            IMailService mailService,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _technologyRepository = technologyRepository;
            _passwordService = passwordService;
            _mailService = mailService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<Dictionary<string, string>> LoginUserAsync(LoginUserDto loginUserDto)
        {
            User userMapped = _mapper.Map<User>(loginUserDto);

            await CheckIfUserExistInDatabaseAsync(userMapped);

            User user = await _userRepository.GetUserWithRoleByEmailAsync(userMapped.Email);

            _passwordService.VerifyPassword(user, loginUserDto);

            await CheckIfAccountIsInactive(user);

            string accessToken = _tokenService.GenerateJwtAccessToken(user);
            RefreshToken refreshToken = _tokenService.GenerateJwtRefreshToken(user);

            //await _tokenService.SaveRefreshTokenAsync(refreshToken, user);

            Dictionary<string, string> tokens = new()
            {
                { "accessToken", accessToken },
                { "refreshToken", refreshToken.Token }
            };

            return tokens;
        }

        private async Task CheckIfAccountIsInactive(User user)
        {
            AccountStatusEnum accountStatusEnum = await _userRepository.GetUserAccountStatus(user);

            if(accountStatusEnum == AccountStatusEnum.INACTIVE)
            {
                //logic to return response in body to change password instead of exception
            }
        }

        private async Task CheckIfUserExistInDatabaseAsync(User user)
        {
            bool existValue = await _userRepository.FindIfUserExistAsync(user);

            if (!existValue)
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_USER_NOTFOUND);
            }
        }

        public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            User createUser = _mapper.Map<User>(registerUserDto);

            await CheckIfUserExistAsync(createUser);

            await SetDefaultRoleToUserAsync(createUser);

            HashTemporaryUserPassword(createUser, createUser.Password);

            User user = await _userRepository.RegisterUserAsync(createUser);

            await _userRepository.SaveChangesAsync();

            UserDto userDto = _mapper.Map<UserDto>(user);

            await _mailService.SendEmailAsync(userDto.Email, createUser.Password);

            return userDto;
        }

        private void HashTemporaryUserPassword(User user, string? password)
        {
            _passwordService.HashPassword(user, password);
        }

        private async Task SetDefaultRoleToUserAsync(User user)
        {
            Role role = await _roleRepository.GetRoleUserAsync();
            user.Role = role;
        }

        private async Task CheckIfUserExistAsync(User user)
        {
            bool existValue = await _userRepository.FindIfUserExistAsync(user);

            if (existValue)
            {
                throw new DataDuplicateException(ExceptionCodeTemplate.BCKND_USER_CONFLICT);
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            List<User> users = await _userRepository.GetAllUsersAsync();

            CheckIfListIsNull(users);

            List<UserDto> usersDto = _mapper.Map<List<UserDto>>(users);

            return usersDto;
        }

        private void CheckIfListIsNull(List<User> users)
        {
            if (users == null)
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_USER_LIST_NOTFOUND);
            }
        }

        public async Task<UserDto> GetUserByIdAsync(long id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);

            CheckIfUserEntityIsNull(user);

            UserDto userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        private void CheckIfUserEntityIsNull(User user)
        {
            if (user == null)
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_USER_NOTFOUND);
            }
        }
        public async Task<UserDto> UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            User user = await _userRepository.GetUserByIdAsync(updateUserDto.Id);

            CheckIfUserEntityIsNull(user);

            UpdateUserProperties(user, updateUserDto);

            await _userRepository.SaveChangesAsync();

            UserDto userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        private void UpdateUserProperties(User user, UpdateUserDto updateUserDto)
        {
            user.Name = updateUserDto.Name;
            user.Surname = updateUserDto.Surname;
        }

        public async Task DeleteUserAsync(long id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);

            CheckIfUserEntityIsNull(user);

            _userRepository.DeleteUser(user);

            await _userRepository.SaveChangesAsync();
        }

        public async Task<UpdateUserTechnologyDto> AssignTechnologyToUserAsync(PatchUserTechnologyDto patchUserTechnologyDto)
        {
            User user = await _userRepository.GetUserWithTechnologiesByIdAsync(patchUserTechnologyDto.IdUser);

            CheckIfUserEntityIsNull(user);

            Technology technology = await _technologyRepository.GetTechnologyByIdAsync(patchUserTechnologyDto.IdTechnology);

            CheckIfTechnologyEntityIsNull(technology);

            CheckIfRelationUserTechnologyExist(user, technology);

            _userRepository.AssignTechnologyToUser(user, technology);

            await _userRepository.SaveChangesAsync();

            UpdateUserTechnologyDto updateUserTechnologyDto = _mapper.Map<UpdateUserTechnologyDto>(user);

            return updateUserTechnologyDto;
        }

        private void CheckIfTechnologyEntityIsNull(Technology technology)
        {
            if (technology == null)
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_TECHNOLOGY_NOTFOUND);
            }
        }

        private void CheckIfRelationUserTechnologyExist(User user, Technology technology)
        {
            if (user.Technologies.Contains(technology))
            {
                throw new DataDuplicateException(ExceptionCodeTemplate.BCKND_RELATION_CONFLICT);
            }
        }

        public async Task UnassignTechnologyFromUserAsync(PatchUserTechnologyDto patchUserTechnologyDto)
        {
            User user = await _userRepository.GetUserWithTechnologiesByIdAsync(patchUserTechnologyDto.IdUser);

            CheckIfUserEntityIsNull(user);

            Technology technology = await _technologyRepository.GetTechnologyByIdAsync(patchUserTechnologyDto.IdTechnology);

            CheckIfTechnologyEntityIsNull(technology);

            CheckIfRelationUserTechnologyNonExist(user, technology);

            _userRepository.UnassignTechnologyFromUser(user, technology);

            await _userRepository.SaveChangesAsync();
        }

        private void CheckIfRelationUserTechnologyNonExist(User user, Technology technology)
        {
            if (!user.Technologies.Contains(technology))
            {
                throw new NotFoundException(ExceptionCodeTemplate.BCKND_RELATION_NOTFOUND);
            }
        }

        public async Task<Dictionary<string, string>> GetRefreshedTokensAsync(RefreshTokenDto refreshTokenDto)
        {
            RefreshToken refreshTokenFromDB = await _tokenService.GetRefreshTokenByTokenAndUserIdAsync(refreshTokenDto.UserId, refreshTokenDto.Token);

            User user = await _userRepository.GetUserByIdAsync(refreshTokenDto.UserId);

            CheckIfUserEntityIsNull(user);

            Dictionary<string, string> tokens = await _tokenService.RefreshTokensAsync(user, refreshTokenFromDB);

            return tokens;
        }

        public async Task LogoutUserAsync()
        {
            await _tokenService.DeactivateCurrentAccessTokenAsync();
        }
    }
}
