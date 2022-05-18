﻿using AutoMapper;
using WorkerManagementAPI.Data.Entities;
using WorkerManagementAPI.Data.Models.UserDtos;
using WorkerManagementAPI.Exceptions;
using WorkerManagementAPI.Services.RoleService.Repository;
using WorkerManagementAPI.Services.TechnologyService.Repository;
using WorkerManagementAPI.Services.UserService.Repository;

namespace WorkerManagementAPI.Services.UserService.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, 
            IRoleRepository roleRepository,
            ITechnologyRepository technologyRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _technologyRepository = technologyRepository;
            _mapper = mapper;
        }

        public async Task LoginUserAsync(LoginUserDto loginUserDto)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            User createUser = _mapper.Map<User>(registerUserDto);

            await CheckIfUserExistAsync(createUser);

            await SetDefaultRoleToUserAsync(createUser);

            User user = await _userRepository.RegisterUserAsync(createUser);

            await _userRepository.SaveChangesAsync();

            UserDto userDto = _mapper.Map<UserDto>(user);

            return userDto;
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
                throw new DataDuplicateException("User already exist");
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
                throw new NotFoundException("List is empty");
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
                throw new NotFoundException("User not found");
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

            _userRepository.SaveChangesAsync();
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
                throw new NotFoundException("Technology not found");
            }
        }

        private void CheckIfRelationUserTechnologyExist(User user, Technology technology)
        {
            if (user.Technologies.Contains(technology))
            {
                throw new DataDuplicateException("Relation already exist");
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
                throw new NotFoundException("Relation not exist");
            }
        }
    }
}
