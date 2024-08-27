using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.DTOs;
using PublicSpaceMaintenanceRequestMS.DTOs.UserDTOs;
using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using PublicSpaceMaintenanceRequestMS.Services.Interfaces;

namespace PublicSpaceMaintenanceRequestMS.Controllers
{
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IApplicationService applicationService, IConfiguration configuration,
            IMapper mapper) : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<UserReadOnlyDTO>> SignupUserAsync(UserSignupDTO? userSignupDTO)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, build a custom response
                var errors = ModelState
                    .Where(e => e.Value!.Errors.Any())
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
                    });

                // instead of return BadRequest(new { Errors = errors });
                throw new InvalidRegistrationException("Errors In Registation: " + errors);
            }

            if (_applicationService == null)
            {
                throw new ServerGenericException("Application Service Null");
            }

            User? userWithUsername = await _applicationService.UserService.GetUserByUsernameAsync(userSignupDTO!.Username!);

            if (userWithUsername is not null)
            {
                throw new UserAlreadyExistsException($"Username {userSignupDTO.Username} already exists.");
            }

            bool emailExists = await _applicationService.UserService.CheckEmailExistsAsync(userSignupDTO!.Email!);

            if (emailExists)
            {
                throw new EmailAlreadyExistsException($"Email {userSignupDTO.Email} already exists.");
            }

            User? returnedUser = await _applicationService.UserService.SignUpUserAsync(userSignupDTO);

            if (returnedUser is null)
            {
                throw new InvalidRegistrationException("Invalid Registration");
            }

            var returnedUserReadOnlyDTO = _mapper.Map<UserReadOnlyDTO>(returnedUser);
            return CreatedAtAction(nameof(GetUserById), new { id = returnedUserReadOnlyDTO.Id }, returnedUserReadOnlyDTO);
        }

        [HttpPost]
        public async Task<ActionResult<JwtTokenDTO>> LoginUserAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _applicationService.UserService.VerifyAndGetUserAsync(userLoginDTO);

            if (user is null)
            {
                throw new UnauthorizedAccessException("Invalid Credentials");
            }

            var userToken = _applicationService.UserService.CreateUserToken(user.Id, user.Username, user.Email,
                user.Role, _configuration["Authentication:SecretKey"]);

            JwtTokenDTO token = new()
            {
                Token = userToken,
                Role = user.Role.ToString(),
                Username = user.Username
            };

            return Ok(token);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadOnlyDTO>> GetUserById(int id)
        {
            var user = await _applicationService.UserService.GetUserByIdAsync(id);

            if (user is null)
            {
                throw new UserNotFoundException($"User with ID {id} not found");
            }

            var returnedUser = _mapper.Map<UserReadOnlyDTO>(user);
            return Ok(returnedUser);
        }


        [HttpPatch("{id}")]
        public async Task<ActionResult<UserReadOnlyDTO>> UpdateUserPatch(int id, UserPatchDTO userPatchDTO)
        {
            var userId = AppUser!.Id;
            if (id != userId)
            {
                throw new ForbiddenException("ForbiddenAccess");
            }

            var updatedUser = await _applicationService.UserService.UpdateUserPatchAsync(userId, userPatchDTO);

            var userReadOnlyDTO = _mapper.Map<UserReadOnlyDTO>(updatedUser);

            return Ok(userReadOnlyDTO);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDTO>> UpdateUserAccount(int id, UserUpdateDTO? userDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value!.Errors.Any())
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value!.Errors.Select(error => error.ErrorMessage).ToArray()
                    });

                throw new InvalidRegistrationException("Errors In Registation: " + errors);
            }

            var updatedUser = await _applicationService.UserService.UpdateUserAsync(id, userDTO!);

            if (updatedUser is null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var updatedUserDTO = _mapper.Map<UserDTO>(updatedUser);
            return Ok(updatedUserDTO);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userId = AppUser!.Id;

            if (id != userId)
            {
                throw new ForbiddenException("Forbidden Access");
            }

            await _applicationService.UserService.DeleteUserAsync(userId);
            return NoContent();
        }


        [HttpGet("{email}")]
        public async Task<IActionResult> CheckDuplicateEmailAsync(string email)
        {
            var emailExists = await _applicationService.UserService.CheckEmailExistsAsync(email);

            if (emailExists)
            {
                // Το email υπάρχει ήδη
                throw new EmailAlreadyExistsException("Email already exists.");
            }
            else
            {
                // Το email δεν υπάρχει
                return Ok(new { msg = "Email is available" });
            }
        }


        [HttpGet("{username}")]
        public async Task<IActionResult> CheckDuplicateUsernameAsync(string username)
        {
            {
                var usernameExists = await _applicationService.UserService.CheckUsernameExistsAsync(username);

                if (usernameExists)
                {
                    // Το username υπάρχει ήδη
                    throw new UsernameAlreadyExistsException("Username already exists.");
                }
                else
                {
                    // Το username δεν υπάρχει
                    return Ok(new { msg = "Username is available" });
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsersFilteredAsync(
            [FromQuery] UserFiltersDTO filtersDTO, int pageNumber, int pageSize)
        {
            var filteredUsers = await _applicationService.UserService
                .GetAllUsersFilteredAsync(pageNumber, pageSize, filtersDTO);

            var users = await _applicationService.UserService.GetAllUsersAsync();

            if (filteredUsers == null || filteredUsers.Count == 0 || users == null || users.Count == 0)
            {
                throw new UserNotFoundException("No users found");
            }

            var totalUsersCount = users.Count;

            var filteredUsersDTOs = _mapper.Map<List<UserDTO>>(filteredUsers);
            return Ok(new { Users = filteredUsersDTOs, TotalCount = totalUsersCount });
        }
    }
}