using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs;
using PublicSpaceMaintenanceRequestMS.Models;
using PublicSpaceMaintenanceRequestMS.Repositories.Interfaces;
using PublicSpaceMaintenanceRequestMS.Security;
using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace PublicSpaceMaintenanceRequestMS.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<UserService>? _logger;
        private readonly IMapper? _mapper;

        public UserService(IUnitOfWork? unitOfWork, ILogger<UserService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="userRole">The role of the user.</param>
        /// <param name="appSecurityKey">The security key used to sign the token.</param>
        /// <returns>A JWT token as a string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any required parameter is null.</exception>
        public string CreateUserToken(int userId, string? username, string? email, UserRole? userRole, string? appSecurityKey)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (userRole == null) throw new ArgumentNullException(nameof(userRole));
            if (string.IsNullOrEmpty(appSecurityKey)) throw new ArgumentNullException(nameof(appSecurityKey));

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey!));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username!),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email!),
                new Claim(ClaimTypes.Role, userRole.ToString()!)
            };

            var jwtSecurityToken = new JwtSecurityToken(
                null, 
                null, 
                claimsInfo, 
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(3), 
                signingCredentials
            );

            // Serialize the token (string)
            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            // return "Bearer " + userToken;
            return userToken;
        }

        /// <summary>
        /// Deletes a user with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the user to be deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        /// <exception cref="UserNotFoundException">Thrown when the user with the specified ID is not found.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the deletion process.</exception>
        public async Task DeleteUserAsync(int id)
        {
            try
            {
                bool result = await _unitOfWork!.UserRepository.DeleteAsync(id);

                if (!result)
                {
                    _logger!.LogWarning($"User with ID {id} not found.");
                    throw new UserNotFoundException(id);
                }

                _logger!.LogInformation($"User with id {id} deleted successfully.");

            } catch (UserNotFoundException e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            } catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a filtered list of users based on the provided filters and pagination parameters.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of users to retrieve per page.</param>
        /// <param name="userFiltersDTO">The filters to apply when retrieving users.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of filtered users as <see cref="User"/> objects.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the retrieval process.</exception>
        public async Task<List<UserDTO>> GetAllUsersFilteredAsync(int pageNumber, int pageSize,
             UserFiltersDTO userFiltersDTO)
        {
            List<User> filteredUsers = new();
            List<Expression<Func<User, bool>>> predicates = new();

            try
            {
                // Add individual predicates for filtering conditions
                if (!string.IsNullOrEmpty(userFiltersDTO.Username))
                {
                    predicates.Add(d => d.Username!.Contains(userFiltersDTO.Username));
                }

                if (userFiltersDTO.Role.HasValue)
                {
                    predicates.Add(d => d.Role == userFiltersDTO.Role.Value);
                }

                filteredUsers = await _unitOfWork!.UserRepository.GetAllUsersFilteredAsync(pageNumber, pageSize, predicates);

                _logger!.LogInformation("{Message}", "Filtered users returned successfully.");

                return _mapper!.Map<List<UserDTO>>(filteredUsers);
            }
            catch (Exception e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a user by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the user.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing the user as a <see cref="User"/> object if found.
        /// </returns>
        /// <exception cref="UserNotFoundException">Thrown when a user with the specified ID is not found.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the retrieval process.</exception>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user;
            try
            {
                user = await _unitOfWork!.UserRepository.GetByIdAsync(id);

                if (user is null) {
                    _logger!.LogWarning($"User with ID {id} not found.");
                    throw new UserNotFoundException(id);
                }

                _logger!.LogInformation($"User with username {user.Username} found and returned");

                return user;

            } catch(UserNotFoundException e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            } catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Retrieves a user by its username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>
        /// A task that represents the asynchronous operation, containing the user as a <see cref="User"/> object if found.
        /// </returns>
        /// <exception cref="UserNotFoundException">Thrown when a user with the specified username is not found.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the retrieval process.</exception>
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            User? user;
            try
            {
                user = await _unitOfWork!.UserRepository.GetByUsernameAsync(username);

                //if (user is null)
                //{
                //    _logger!.LogWarning($"User with username {username} not found.");
                //    throw new UserNotFoundException($"User with username {username} not found.");
                //}
                _logger!.LogInformation("{Message}", "User: " + user + " found and returned");

                return user;

            //} catch(UserNotFoundException e)
            //{
            //    _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
            //    throw;
            } catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Updates the role of a user specified by its ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose role is to be updated.</param>
        /// <param name="userRole">The new role to be assigned to the user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="UserNotFoundException">Thrown when a user with the specified ID is not found.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the role update process.</exception>
        public async Task UpdateUserRoleAsync(int userId, UserRole userRole)
        {
            try
            {
                bool result = await _unitOfWork!.UserRepository.UpdateUserRoleAsync(userId, userRole);

                if (!result)
                {
                    _logger!.LogWarning($"User with ID {userId} not found");
                    throw new UserNotFoundException(userId);
                }

                _logger!.LogInformation($"User with ID {userId} found and its role updated to {userRole}.");

            } catch (UserNotFoundException e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            } catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Registers a new user in the system based on the provided signup data.
        /// </summary>
        /// <param name="signupDTO">The data transfer object containing user signup information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly registered user or null if registration fails.</returns>
        /// <exception cref="UserAlreadyExistsException">Thrown when a user with the given username or email already exists.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the registration process.</exception>
        public async Task<User?> SignUpUserAsync(UserSignupDTO signupDTO)
        {
            Citizen? citizen;
            Officer? officer;
            Admin? admin;
            User? existingUserWithUsername;
            User? newUser;

            try {
                newUser = _mapper!.Map<User>(signupDTO);

                newUser.Password = EncryptionUtil.Encrypt(signupDTO.Password!);

                // Checks if a user with the given username already exists
                existingUserWithUsername = await _unitOfWork!.UserRepository.GetByUsernameAsync(newUser.Username!);

                //Checks if the given email already exists
                var existingUserWithEmail = await _unitOfWork.UserRepository.GetByEmailAsync(newUser.Email!);
                
                if (existingUserWithUsername != null)
                {
                    _logger!.LogWarning($"A user with username {newUser.Username} already exists.");
                    throw new UserAlreadyExistsException(newUser.Username);
                }
                if (existingUserWithEmail != null)
                {
                    _logger!.LogWarning($"A user with email {newUser.Email} already exists.");
                    throw new UserAlreadyExistsException(newUser.Email);
                }

                // Assign roles and related entities
                if (newUser.Role == UserRole.Citizen)
                {
                    citizen = _mapper.Map<Citizen>(signupDTO);
                    newUser.Citizen = citizen;
                }
                
                if (newUser.Role == UserRole.Officer)
                {
                    officer = _mapper.Map<Officer>(signupDTO);
                    officer.Department = await _unitOfWork.DepartmentRepository.GetByIdAsync(signupDTO.DepartmentId);
                    newUser.Officer = officer;
                }

                if (newUser.Role == UserRole.Admin)
                {
                    admin = _mapper.Map<Admin>(signupDTO);
                    newUser.Admin = admin;
                }

                // Adds the new user to the DB
                await _unitOfWork.UserRepository.AddAsync(newUser);

                // Save changes to the database
                if (!await _unitOfWork.SaveAsync())
                {
                    throw new Exception("Failed to save user to the database.");
                }

                _logger!.LogInformation($"User with username {newUser.Username} has been successfully registered.");

                return newUser;

            } catch (UserAlreadyExistsException e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Updates the details of an existing user based on the provided user data.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="userUpdateDTO">The data transfer object containing the updated user information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated user data as a <see cref="User"/> or null if the update fails.</returns>
        /// <exception cref="UserNotFoundException">Thrown when a user with the given ID is not found.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the update process.</exception>
        public async Task<User?> UpdateUserAsync(int userId, UserUpdateDTO userUpdateDTO)
        {
            User? existingUser;
            User? userToReturn;

            try
            {
                existingUser = await _unitOfWork!.UserRepository.GetByIdAsync(userId);

                if (existingUser is null) {
                    _logger!.LogWarning($"User with ID {userId} not found");
                    throw new UserNotFoundException(userId);
                }

                // Ενημέρωση μόνο των πεδίων που δεν είναι null στο DTO
                if (!string.IsNullOrEmpty(userUpdateDTO.Username))
                {
                    existingUser.Username = userUpdateDTO.Username;
                }

                if (!string.IsNullOrEmpty(userUpdateDTO.Password))
                {
                    existingUser.Password = EncryptionUtil.Encrypt(userUpdateDTO.Password);
                }

                if (!string.IsNullOrEmpty(userUpdateDTO.Email))
                {
                    existingUser.Email = userUpdateDTO.Email;
                }

                if (!string.IsNullOrEmpty(userUpdateDTO.Firstname))
                {
                    existingUser.Firstname = userUpdateDTO.Firstname;
                }

                if (!string.IsNullOrEmpty(userUpdateDTO.Lastname))
                {
                    existingUser.Lastname = userUpdateDTO.Lastname;
                }

                if (!string.IsNullOrEmpty(userUpdateDTO.Phonenumber))
                {
                    existingUser.Phonenumber = userUpdateDTO.Phonenumber;
                }

                // if role = officer
                if (userUpdateDTO.Role != default)
                {
                    existingUser.Role = userUpdateDTO.Role;
                    if (userUpdateDTO.Role == UserRole.Officer)
                    {
                        if (existingUser.Officer == null)
                        {
                            existingUser.Officer = new Officer();
                        }

                        existingUser.Officer.DepartmentId = userUpdateDTO.Officer?.DepartmentId ?? existingUser.Officer.DepartmentId;
                    }
                    else
                    {
                        existingUser.Officer = null; // Καθαρισμός της αναφοράς Officer αν ο ρόλος δεν είναι Officer
                    }
                }

                // Update entity
                userToReturn = await _unitOfWork!.UserRepository.UpdateUserAsync(userId, existingUser);

                // Save changes
                await _unitOfWork.SaveAsync();

                _logger!.LogInformation("{Message}", $"User with ID {userToReturn!.Id} updated successfully");

                return userToReturn;
            }
            catch (UserNotFoundException e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Partially updates the details of an existing user based on the provided patch data.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="userPatchDTO">The data transfer object containing the patch information for the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated user data as a <see cref="User"/> or null if the update fails.</returns>
        /// <exception cref="UserNotFoundException">Thrown when a user with the given ID is not found.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the update process.</exception>
        public async Task<User?> UpdateUserPatchAsync(int userId, UserPatchDTO userPatchDTO)
        {
            User? existingUser;

            try {
                existingUser = await _unitOfWork!.UserRepository.GetByIdAsync(userId);

                if (existingUser is null)
                {
                    _logger!.LogWarning($"User with ID {userId} not found.");
                    throw new UserNotFoundException(userId);
                }

                // validation of current password if given
                if (!string.IsNullOrEmpty(userPatchDTO.CurrentPassword))
                {
                    if (!EncryptionUtil.IsValidPassword(userPatchDTO.CurrentPassword, existingUser.Password!))
                    {
                        throw new UnauthorizedAccessException("Invalid current password");
                    }

                    if (!string.IsNullOrEmpty(userPatchDTO.NewPassword))
                    {
                        existingUser.Password = EncryptionUtil.Encrypt(userPatchDTO.NewPassword!);
                    }
                }

                if (!string.IsNullOrEmpty(userPatchDTO.Username))
                {
                    existingUser.Username = userPatchDTO.Username;
                }

                if (!string.IsNullOrEmpty(userPatchDTO.Email))
                {
                    existingUser.Email = userPatchDTO.Email;
                }

                var updatedUser = await _unitOfWork.UserRepository.UpdateUserAsync(userId, existingUser);
                await _unitOfWork.SaveAsync();

                _logger!.LogInformation("{Message}", $"User with username {existingUser.Username} updated successfully.");

                return updatedUser;
            }
            catch (UserNotFoundException e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Verifies the user credentials and retrieves the user details if valid.
        /// </summary>
        /// <param name="userLoginDTO">The data transfer object containing the login credentials.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="User"/> if the credentials are valid, or null if invalid.</returns>
        /// <exception cref="UserNotFoundException">Thrown when a user with the provided credentials is not found.</exception>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the verification process.</exception>
        public async Task<User?> VerifyAndGetUserAsync(UserLoginDTO userLoginDTO)
        {
            User? user = null;

            try
            {
                user = await _unitOfWork!.UserRepository.GetByCredentialsAsync(userLoginDTO.Username!, userLoginDTO.Password!);
                
                if (user is null)
                {
                    _logger!.LogWarning($"User with username {userLoginDTO.Username} not found.");
                    throw new UserNotFoundException($"User with username {userLoginDTO.Username} not found");
                }
                
                _logger!.LogInformation("{Message}", $"User with username {userLoginDTO.Username} found and returned");
                
                return user;
            }
            catch (UserNotFoundException e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Checks if a given email already exists.
        /// </summary>
        /// <param name="email">The given email to check for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the email already exists, false otherwise<returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the check process.</exception>
        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            try
            {
                var user = await _unitOfWork!.UserRepository.GetByEmailAsync(email);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        /// <summary>
        /// Checks if a given username already exists.
        /// </summary>
        /// <param name="email">The given username to check for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the username already exists, false otherwise<returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs during the check process.</exception>
        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            try
            {
                var user = await _unitOfWork!.UserRepository.GetByUsernameAsync(username);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                var totalUsers = await _unitOfWork!.UserRepository.GetAllAsync();

                if (totalUsers == null)
                {
                    _logger!.LogWarning($"No users found.");
                    throw new UserNotFoundException("No users found");
                }

                _logger!.LogInformation($"{totalUsers.Count()} users found and returned");

                var totalUsersDTOs = _mapper!.Map<List<UserDTO>>(totalUsers);

                return totalUsersDTOs;
            }
            catch (UserNotFoundException e)
            {
                _logger!.LogError("{Message}{Exception}", e.Message, e.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<int?> GetCitizenIdByUsernameAsync(string username)
        {
            try
            {
                // Validate username
                if (string.IsNullOrEmpty(username))
                {
                    _logger!.LogWarning("Username is null or empty.");
                    return null;
                }

                // Retrieve user by username
                var user = await _unitOfWork!.UserRepository.GetByUsernameAsync(username);

                if (user == null)
                {
                    _logger!.LogWarning($"User with username {username} not found.");
                    return null;
                }

                // Retrieve citizenId from database using userId
                var citizen = await _unitOfWork.CitizenRepository.GetByIdAsync(user.Id);

                if (citizen == null)
                {
                    _logger!.LogWarning($"Citizen with userId {user.Id} not found.");
                    return null;
                }

                _logger!.LogInformation($"CitizenId {citizen.Id} found for username {username}.");
                return citizen.Id;
            }
            catch (Exception ex)
            {
                _logger!.LogError("An error occurred while retrieving the citizen ID. {Exception}", ex.StackTrace);
                throw;
            }
        }
    }
}
