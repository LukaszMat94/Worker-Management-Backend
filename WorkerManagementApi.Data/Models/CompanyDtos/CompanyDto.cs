using WorkerManagementAPI.Data.Models.UserDtos;

namespace WorkerManagementAPI.Data.Models.CompanyDtos
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
