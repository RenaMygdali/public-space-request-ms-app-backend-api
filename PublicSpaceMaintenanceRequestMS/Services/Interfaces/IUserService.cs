using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs;
using PublicSpaceMaintenanceRequestMS.Models;

namespace PublicSpaceMaintenanceRequestMS.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> SignUpUserAsync(UserSignupDTO request);
        Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials);
        Task<User?> UpdateUserAsync(int userId, UserUpdateDTO userUpdateDTO);
        Task<User?> UpdateUserPatchAsync(int userId, UserPatchDTO request);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<List<UserDTO>> GetAllUsersFilteredAsync(int pageNumber, int pageSize,
             UserFiltersDTO userFiltersDTO);
        Task UpdateUserRoleAsync(int userId, UserRole userRole);
        Task DeleteUserAsync(int id);
        string CreateUserToken(int userId, string? username, string? email, UserRole? userRole,
            string? appSecurityKey);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<int?> GetCitizenIdByUsernameAsync(string username);
    }
}
