using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs.AdminDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.CitizenDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.OfficerDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.RequestDTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs;
using PublicSpaceMaintenanceRequestMS.Models;
using PublicSpaceMaintenanceRequestMS.Repositories;
using PublicSpaceMaintenanceRequestMS.Security;
using PublicSpaceMaintenanceRequestMS.Services;
using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using PublicSpaceMaintenanceRequestMS.Tests.Fixtures;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PublicSpaceMaintenanceRequestMS.Tests.Services
{
    public class UserServiceTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly UserService? _userService;
        private readonly PublicSpaceMaintenanceRequestDbDBContext? _context;
        private readonly UnitOfWork? _unitOfWork;
        private readonly ILogger<UserService>? _logger; // Mock ILogger
        private readonly IMapper? _mapper;

        public UserServiceTests(TestDatabaseFixture fixture)
        {
            _context = fixture.GetContext();

            // Ρύθμιση AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                // Mapping User to various DTOs
                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<User, OfficerDTO>().ReverseMap();
                cfg.CreateMap<User, AdminDTO>().ReverseMap();
                cfg.CreateMap<User, CitizenDTO>().ReverseMap();
                cfg.CreateMap<User, UserPatchDTO>().ReverseMap();
                cfg.CreateMap<User, UserReadOnlyDTO>().ReverseMap();
                cfg.CreateMap<User, UserFiltersDTO>().ReverseMap();

                // Mapping UserSignupDTO to User
                cfg.CreateMap<UserSignupDTO, User>()
                    .ForMember(dest => dest.Citizen, opt => opt.Ignore())
                    .ForMember(dest => dest.Officer, opt => opt.Ignore())
                    .ForMember(dest => dest.Admin, opt => opt.Ignore());

                // Mapping UserSignupDTO to specific user roles
                cfg.CreateMap<UserSignupDTO, Citizen>().ReverseMap();
                cfg.CreateMap<UserSignupDTO, Officer>()
                    .ForMember(dest => dest.Department, opt => opt.MapFrom(src => new Department { Id = src.DepartmentId }));
                cfg.CreateMap<UserSignupDTO, Admin>().ReverseMap();

                cfg.CreateMap<UserUpdateDTO, User>();

                cfg.CreateMap<UserPatchDTO, User>()
                .AfterMap((src, dest) =>
                {
                    // Ενημερώνουμε μόνο τα πεδία που δεν είναι null
                    if (!string.IsNullOrEmpty(src.Username))
                    {
                        dest.Username = src.Username;
                    }

                    if (!string.IsNullOrEmpty(src.Email))
                    {
                        dest.Email = src.Email;
                    }

                    if (!string.IsNullOrEmpty(src.NewPassword))
                    {
                        dest.Password = EncryptionUtil.Encrypt(src.NewPassword); // Χρησιμοποιούμε τον κωδικό πρόσβασης
                    }
                });


                // Mapping Request to various DTOs
                cfg.CreateMap<Request, RequestDTO>().ReverseMap();
                cfg.CreateMap<Request, RequestUpdateDTO>().ReverseMap();
                cfg.CreateMap<Request, RequestReadOnlyDTO>().ReverseMap();
            });

            _mapper = config.CreateMapper();

            _unitOfWork = new UnitOfWork(_context, _mapper);

            _logger = new LoggerFactory().CreateLogger<UserService>(); // Mock ILogger

            // Δημιουργία της UserService
            _userService = new UserService(_unitOfWork, _logger, _mapper);
        }

        [Fact]
        public async Task CheckEmailExistsAsync_EmailExists_ThrowsException()
        {
            // Arrange
            var email = "exists@email.com";

            var user = new User
            {
                Firstname = "UserExists",
                Email = email
            };

            await _unitOfWork!.UserRepository.AddAsync(user);
            await _unitOfWork!.SaveAsync();


            // Act & Assert
            await Assert.ThrowsAsync<EmailAlreadyExistsException>(() => _userService!.CheckEmailExistsAsync(email));
        }


        [Fact]
        public async Task CheckEmailExistsAsync_EmailNotExists_ReturnsFalse()
        {
            // Arrange
            var email = "notExistent@email.com";

            // Act
            bool emailExists = await _userService!.CheckEmailExistsAsync(email);

            // Assert
            Assert.False(emailExists);
        }

        [Fact]
        public void CreateUserToken_ValidParameters_ReturnsToken()
        {
            // Arrange
            var userId = 4;
            var username = "testUser";
            var email = "testuser@example.com";
            var userRole = UserRole.Citizen;
            var appSecurityKey = "1cab7fff5664292e0c1db3c1fc8ccc6c41d3b76f403b2a109d07bc70b18329ce"; // Ensure this matches the actual key length used in your system

            // Act
            var token = _userService!.CreateUserToken(userId, username, email, userRole, appSecurityKey);

            // Assert
            Assert.NotNull(token);

            // Validate the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            Assert.Equal(username, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal(userId.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            Assert.Equal(email, jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value);
            Assert.Equal(userRole.ToString(), jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value);

            // Validate token expiration
            var exp = jwtToken.ValidTo;
            Assert.True(exp > DateTime.UtcNow, "Token expiration should be in the future");
            Assert.True(exp < DateTime.UtcNow.AddHours(3), "Token expiration should be within 3 hours from now");

            // Optionally validate signature (if you have access to the key used)
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey))
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            Assert.NotNull(principal);
            Assert.Equal(username, principal.FindFirst(ClaimTypes.Name)?.Value);
            Assert.Equal(userId.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal(email, principal.FindFirst(ClaimTypes.Email)?.Value);
            Assert.Equal(userRole.ToString(), principal.FindFirst(ClaimTypes.Role)?.Value);
        }


    [Fact]
        public async Task DeleteUserAsync_UserExists_DeleteUser()
        {
            // Arrange
            var userId = 1;

            // Act
            await _userService!.DeleteUserAsync(userId);

            // Assert
            var deletedUser = await _context!.Users.FindAsync(userId);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task GetAllUsersFilteredAsync_ValidFilters_ReturnsFilteredUsers()
        {
            // Arrange
            var user1 = new User
            {
                Username = "testUser",
                Email = "test@example.com",
                Role = UserRole.Citizen
            };

            var user2 = new User
            {
                Username = "otherUser",
                Email = "other@example.com",
                Role = UserRole.Admin
            };

            await _unitOfWork!.UserRepository.AddAsync(user1);
            await _unitOfWork!.UserRepository.AddAsync(user2);
            await _unitOfWork!.SaveAsync();

            // Paging & Filters
            var pageNumber = 1;
            var pageSize = 10;
            var userFiltersDTO = new UserFiltersDTO
            {
                Username = "testUser",
                Role = UserRole.Citizen
            };

            // Act
            var result = await _userService!.GetAllUsersFilteredAsync(pageNumber, pageSize, userFiltersDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Ensure only one user matches the filter criteria
            Assert.Equal("testUser", result.First().Username);
            Assert.Equal(UserRole.Citizen, result.First().Role);
        }

        [Fact]
        public async Task GetAllUsersFilteredAsync_NoUsersFound_ReturnsEmptyList()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var userFiltersDTO = new UserFiltersDTO
            {
                Username = "nonExistentUser"
            };

            // Act
            var result = await _userService!.GetAllUsersFilteredAsync(pageNumber, pageSize, userFiltersDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // Ensure the result is an empty list
        }


        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = 2;

            // Act
            var user = await _userService!.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
            Assert.Equal("user2", user.Username);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_UserExists_ReturnUser()
        {
            // Arrange
            var username = "user2";

            // Act
            var user = await _userService!.GetUserByUsernameAsync(username);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(2, user.Id);
            Assert.Equal(UserRole.Officer, user.Role);
        }

        [Fact]
        public async Task UpdateUserAsync_UserExists_UpdatesUser()
        {
            // Arrange
            var userId = 1;
            var password = "updatedpassword123";

            var userUpdateDTO = new UserUpdateDTO
            {
                Id = 1,
                Username = "updatedUser",
                Email = "updated@example.com",
                Firstname = "Updated",
                Lastname = string.Empty,
                Password = password,
                Phonenumber = "1234567890"
            };

            // Act
            var result = await _userService!.UpdateUserAsync(userId, userUpdateDTO);
            var updatedUser = await _context!.Users.FindAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(updatedUser);
            Assert.Equal(userUpdateDTO.Username, updatedUser.Username);
            Assert.Equal(userUpdateDTO.Email, updatedUser.Email);
            Assert.Equal(userUpdateDTO.Phonenumber, updatedUser.Phonenumber);
            Assert.True(EncryptionUtil.IsValidPassword(password, updatedUser.Password!));
            Assert.Equal("Last1", updatedUser.Lastname);
        }

        [Fact]
        public async Task UpdateUserAsync_UserNotExists_NotUpdateUser()
        {
            // Arrange
            var userId = 534;

            var userUpdateDTO = new UserUpdateDTO
            {
                Id = 534,
                Username = "updatedUser",
                Email = "updated@example.com",
                Firstname = "Updated",
                Lastname = "User",
                Password = "updatedpassword123",
                Phonenumber = "1234567890"
            };

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService!.UpdateUserAsync(userId, userUpdateDTO));
        }

        [Fact]
        public async Task UpdateUserPatchAsync_ValidUser_UpdatesSuccessfully()
        {
            // Arrange
            var currentPassword = "OldPass123!";
            var encryptedCurrentPassword = EncryptionUtil.Encrypt(currentPassword);

            var existingUser = new User
            {
                Id = 43,
                Username = "oldUsername",
                Email = "oldemail@example.com",
                Password = encryptedCurrentPassword,
                Role = UserRole.Citizen
            };

            await _unitOfWork!.UserRepository.AddAsync(existingUser);
            await _unitOfWork.SaveAsync();

            var newPassword = "NewPass123!";

            var userPatchDTO = new UserPatchDTO
            {
                Username = string.Empty,
                Email = "newemail@example.com",
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            // Act
            var updatedUser = await _userService!.UpdateUserPatchAsync(existingUser.Id, userPatchDTO);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal(existingUser.Username, updatedUser.Username);
            Assert.Equal(userPatchDTO.Email, updatedUser.Email);
            Assert.True(EncryptionUtil.IsValidPassword(newPassword, updatedUser.Password!));
        }

        [Fact]
        public async Task UpdateUserPatchAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var userPatchDTO = new UserPatchDTO
            {
                Username = "nonExistentUsername",
                Email = "nonexistentemail@example.com"
            };

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService!.UpdateUserPatchAsync(9999, userPatchDTO)); // Assume 9999 is a non-existent user ID
        }

        [Fact]
        public async Task UpdateUserPatchAsync_EmptyNewPassword_DoesNotEncrypt()
        {
            // Arrange
            var currentPassword = "OldPass123!";
            var encryptedCurrentPassword = EncryptionUtil.Encrypt(currentPassword);

            var existingUser = new User
            {
                Id = 35,
                Username = "testUser",
                Email = "test@example.com",
                Password = encryptedCurrentPassword,
                Role = UserRole.Citizen
            };

            await _unitOfWork!.UserRepository.AddAsync(existingUser);
            await _unitOfWork.SaveAsync();

            var userPatchDTO = new UserPatchDTO
            {
                Username = "updatedUser",
                CurrentPassword = currentPassword,
                NewPassword = string.Empty // No new password provided
            };

            // Act
            var updatedUser = await _userService!.UpdateUserPatchAsync(existingUser.Id, userPatchDTO);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal("updatedUser", updatedUser.Username);
            Assert.Equal(existingUser.Email, updatedUser.Email);
            Assert.Equal(existingUser.Password, updatedUser.Password); // Password should remain the same
            Assert.True(EncryptionUtil.IsValidPassword(currentPassword, updatedUser.Password!));
            Assert.False(EncryptionUtil.IsValidPassword(userPatchDTO.NewPassword, updatedUser.Password!));
        }

        [Fact]
        public async Task UpdateUserPatchAsync_EmptyCurrentPassword_DoesNotEncrypt()
        {
            // Arrange
            var currentPassword = "OldPass123!";
            var encryptedCurrentPassword = EncryptionUtil.Encrypt(currentPassword);

            var existingUser = new User
            {
                Id = 35,
                Username = "testUser",
                Email = "test@example.com",
                Password = encryptedCurrentPassword,
                Role = UserRole.Citizen
            };

            await _unitOfWork!.UserRepository.AddAsync(existingUser);
            await _unitOfWork.SaveAsync();

            var newPassword = "NewPass123";

            var userPatchDTO = new UserPatchDTO
            {
                Username = "updatedUser",
                CurrentPassword = string.Empty, // no current password provided
                NewPassword = newPassword 
            };

            // Act
            var updatedUser = await _userService!.UpdateUserPatchAsync(existingUser.Id, userPatchDTO);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal(userPatchDTO.Username, updatedUser.Username);
            Assert.Equal(existingUser.Email, updatedUser.Email);
            Assert.Equal(existingUser.Password, updatedUser.Password); // Password should remain the same
            Assert.True(EncryptionUtil.IsValidPassword(currentPassword, updatedUser.Password!));
            Assert.False(EncryptionUtil.IsValidPassword(userPatchDTO.NewPassword, updatedUser.Password!));
        }


        [Fact]
        public async Task UpdateUserRoleAsync()
        {
            // Arrange
            var userId = 10;
            var newUser = new User
            {
                Id = 10,
                Username = "user6",
                Role = UserRole.Admin
            };

            await _unitOfWork!.UserRepository.AddAsync(newUser);
            await _unitOfWork!.SaveAsync();

            // Act
            await _userService!.UpdateUserRoleAsync(userId, UserRole.Officer);

            // Assert
            Assert.Equal(UserRole.Officer, newUser.Role);
            Assert.Equal("user6", newUser.Username);
        }

        [Fact]
        public async Task SignUpUserAsync_UserDoesNotExist_CreateUser()
        {
            // Arrange
            var password = "SecurePass123!";

            var signupDTO = new UserSignupDTO
            {
                Username = "newUser",
                Email = "newuser@example.com",
                Password = password,
                Firstname = "John",
                Lastname = "Doe",
                Phonenumber = "1234567890",
                Role = UserRole.Citizen
            };

            // Act
            var result = await _userService!.SignUpUserAsync(signupDTO);

            var newUser = await _context!.Users.FindAsync(result!.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("newUser", newUser!.Username);
            Assert.Equal("newuser@example.com", newUser.Email);
            Assert.True(EncryptionUtil.IsValidPassword(password, newUser!.Password!)); // Check password encryption if needed
            Assert.Equal("John", result.Firstname);
            Assert.Equal("Doe", result.Lastname);
            Assert.Equal("1234567890", newUser.Phonenumber);
            Assert.Equal(UserRole.Citizen, result.Role);
            Assert.Equal("newUser", newUser!.Username);
        }

        [Fact]
        public async Task SignUpUserAsync_UsernameAlreadyExists_ThrowsUserAlreadyExistsException()
        {
            // Arrange
            var existingUser = new User
            {
                Username = "existingUser",
                Email = "existinguser@example.com",
                Password = "SecurePass123!",
                Role = UserRole.Citizen
            };
            await _unitOfWork!.UserRepository.AddAsync(existingUser);
            await _unitOfWork.SaveAsync();

            var signupDTO = new UserSignupDTO
            {
                Username = "existingUser",
                Email = "newemail@example.com",
                Password = "SecurePass123!",
                Firstname = "John",
                Lastname = "Doe",
                Phonenumber = "1234567890",
                Role = UserRole.Citizen
            };

            // Act & Assert
            await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _userService!.SignUpUserAsync(signupDTO));
        }

        [Fact]
        public async Task SignUpUserAsync_EmailAlreadyExists_ThrowsUserAlreadyExistsException()
        {
            // Arrange
            var existingUser = new User
            {
                Username = "uniqueUser",
                Email = "existingemail@example.com",
                Password = "SecurePass123!",
                Role = UserRole.Citizen
            };
            await _unitOfWork!.UserRepository.AddAsync(existingUser);
            await _unitOfWork.SaveAsync();

            var signupDTO = new UserSignupDTO
            {
                Username = "newUser",
                Email = "existingemail@example.com",
                Password = "SecurePass123!",
                Firstname = "John",
                Lastname = "Doe",
                Phonenumber = "1234567890",
                Role = UserRole.Citizen
            };

            // Act & Assert
            await Assert.ThrowsAsync<UserAlreadyExistsException>(() => _userService!.SignUpUserAsync(signupDTO));
        }

        [Fact]
        public async Task SignUpUserAsync_OfficerRole_CreatesOfficerWithDepartment()
        {
            // Arrange
            var signupDTO = new UserSignupDTO
            {
                Username = "newOfficer",
                Email = "newofficer@example.com",
                Password = "Password123!",
                Firstname = "New",
                Lastname = "Officer",
                Phonenumber = "1234567890",
                DepartmentId = 1, // This department exists in TestDatabaseFixture
                Role = UserRole.Officer
            };

            // Act
            var newUser = await _userService!.SignUpUserAsync(signupDTO);
            await _unitOfWork!.SaveAsync();

            // Assert
            Assert.NotNull(newUser);
            Assert.Equal(signupDTO.Username, newUser.Username);
            Assert.Equal(signupDTO.Email, newUser.Email);
            Assert.Equal(signupDTO.Firstname, newUser.Firstname);
            Assert.Equal(signupDTO.Lastname, newUser.Lastname);
            Assert.Equal(signupDTO.Phonenumber, newUser.Phonenumber);
            Assert.NotNull(newUser.Officer);
            Assert.Equal(signupDTO.DepartmentId, newUser.Officer!.DepartmentId);
        }

        [Fact]
        public async Task VerifyAndGetUserAsync_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Username = "validUser",
                Password = EncryptionUtil.Encrypt("ValidPass123!"), // Ensure the password is encrypted
                Email = "validuser@example.com",
                Role = UserRole.Citizen
            };

            await _unitOfWork!.UserRepository.AddAsync(user);
            await _unitOfWork.SaveAsync();

            var userLoginDTO = new UserLoginDTO
            {
                Username = "validUser",
                Password = "ValidPass123!"
            };

            // Act
            var result = await _userService!.VerifyAndGetUserAsync(userLoginDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("validUser", result.Username);
            Assert.Equal("validuser@example.com", result.Email);
            Assert.Equal(UserRole.Citizen, result.Role);
            Assert.True(EncryptionUtil.IsValidPassword("ValidPass123!", result.Password!));
        }

        [Fact]
        public async Task VerifyAndGetUserAsync_InvalidUsername_ReturnsNull()
        {
            // Arrange
            var userLoginDTO = new UserLoginDTO
            {
                Username = "nonExistentUser",
                Password = "SomePassword123!"
            };

            // Act
            var result = await _userService!.VerifyAndGetUserAsync(userLoginDTO);

            // Assert
            Assert.Null(result); // Expecting null because the username doesn't exist
        }

        [Fact]
        public async Task VerifyAndGetUserAsync_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var user = new User
            {
                Username = "validUser",
                Password = EncryptionUtil.Encrypt("ValidPass123!"), // Ensure the password is encrypted
                Email = "validuser@example.com",
                Role = UserRole.Citizen
            };

            // Add the user to the database
            await _unitOfWork!.UserRepository.AddAsync(user);
            await _unitOfWork.SaveAsync();

            var userLoginDTO = new UserLoginDTO
            {
                Username = "validUser",
                Password = "InvalidPass123!" // Incorrect password
            };

            // Act
            var result = await _userService!.VerifyAndGetUserAsync(userLoginDTO);

            // Assert
            Assert.Null(result); // Expecting null because the password is incorrect
        }
    }
}
