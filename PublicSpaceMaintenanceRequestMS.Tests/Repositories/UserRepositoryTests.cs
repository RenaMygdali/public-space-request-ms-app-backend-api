using Microsoft.EntityFrameworkCore;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;
using PublicSpaceMaintenanceRequestMS.Repositories;
using PublicSpaceMaintenanceRequestMS.Security;
using PublicSpaceMaintenanceRequestMS.Tests.Fixtures;
using System.Linq.Expressions;

namespace PublicSpaceMaintenanceRequestMS.Tests.Repositories
{
    public class UserRepositoryTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly TestDatabaseFixture _fixture;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests(TestDatabaseFixture fixture)
        {
            _fixture = fixture;

            _userRepository = new UserRepository(_fixture.GetContext());
        }


        // BaseRepository API tests

        [Fact]
        public async Task AddAsync_ShouldAddNewUser()
        {
            // Arrange
            var user3 = new User
            {
                Username = "user3",
                Password = "password3",
                Firstname = "First3",
                Lastname = "Last3",
                Email = "user3@example.com",
                Phonenumber = "1234567890",
                Role = UserRole.Citizen
            };

            // Act
            await _userRepository.AddAsync(user3);
            await _fixture.GetContext().SaveChangesAsync();

            // Assert
            var addedUser = await _fixture.GetContext().Users.FindAsync(user3.Id);

            Assert.NotNull(addedUser);
            Assert.Equal("user3", addedUser.Username);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
            // Arrange
            var userId = 1;

            var updatedUser = new User
            {
                Id = userId,
                Username = "updatedUsername1"
            };

            // Act

            var result = await _userRepository.UpdateUserAsync(userId, updatedUser);

            // Assert
            var updatedUser1 = await _fixture.GetContext().Users.FindAsync(userId);

            Assert.NotNull(result);
            Assert.NotNull(updatedUser1);
            Assert.Equal("updatedUsername1", updatedUser1.Username);
        }

        [Fact]
        public async Task AddRangeAsync_ShouldAddRangeOfUsers()
        {
            // Arrange
            var newUsers = new List<User>
            {
                new User
                {
                    Username = "user4",
                    Password = "password123",
                    Firstname = "First4",
                    Lastname = "Last4",
                    Email = "user4@example.com",
                    Phonenumber = "1234567890",
                    Role = UserRole.Admin
                },
                new User
                {
                    Username = "user5",
                    Password = "Password456",
                    Firstname = "First5",
                    Lastname = "Last5",
                    Email = "user4@example.com",
                    Phonenumber = "0987654321",
                    Role = UserRole.Admin
                }
            };

            // Act
            await _userRepository.AddRangeAsync(newUsers);
            await _fixture.GetContext().SaveChangesAsync();

            // Assert
            //var totalUsers = await _fixture.GetContext().Users.ToListAsync();
            var addedUser4 = await _fixture.GetContext().Users.FirstOrDefaultAsync(u => u.Username == "user4");
            var addedUser5 = await _fixture.GetContext().Users.FirstOrDefaultAsync(u => u.Username == "user5");

            Assert.NotNull(addedUser4);
            Assert.NotNull(addedUser5);
            Assert.Equal("user4", addedUser4.Username);
            Assert.Equal("user5", addedUser5.Username);
        } 

        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser()
        {
            // Arrange
            var userToDelete = await _fixture.GetContext().Users.FindAsync(1);
            Assert.NotNull(userToDelete);

            // Act
            var result = await _userRepository.DeleteAsync(userToDelete.Id);
            await _fixture.GetContext().SaveChangesAsync();

            // Assert
            Assert.True(result); // Assert that deletion was successful

            // Check if user was deleted
            var deletedUser = await _fixture.GetContext().Users.FindAsync(userToDelete.Id);

            Assert.Null(deletedUser);       // Assert that user is not found in DB.
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var context = _fixture.GetContext();
            var users = await context.Users.ToListAsync();

            // Act
            var result = await _userRepository.GetAllAsync();

            // Assert
            Assert.Equal(users.Count, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            // Arrange
            var userId = 2;

            // Act
            var userToReturn = await _userRepository.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(userToReturn);
            Assert.Equal(UserRole.Officer, userToReturn.Role);

        }

        [Fact]
        public async Task GetCountAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            var usersToCount = await _fixture.GetContext().Users.CountAsync();

            // Act
            var result = await _userRepository.GetCountAsync();

            // Assert
            Assert.Equal(usersToCount, result);
        }

        [Fact]
        public async Task GetAllFilteredAsync_ShouldReturnFilteredUsers()
        {
            // Arrange
            var newUser = new User
            {
                Username = "userFiltered",
                Password = "password",
                Firstname = "Filtered",
                Lastname = "User",
                Email = "userfiltered@example.com",
                Phonenumber = "1234567890",
                Role = UserRole.Citizen
            };

            await _userRepository.AddAsync(newUser);
            await _fixture.GetContext().SaveChangesAsync();

            // Act
            var pageNumber = 0;
            var pageSize = 10;
            var predicates = new List<Expression<Func<User, bool>>>
            {
                u => u.Role == UserRole.Citizen,
                u => u.Firstname!.StartsWith("Filt")
            };

            var filteredUsers = await _userRepository.GetAllUsersFilteredAsync(pageNumber, pageSize, predicates);

            // Assert
            Assert.NotNull(filteredUsers);
            Assert.Single(filteredUsers);
            Assert.Equal("Filtered", filteredUsers[0].Firstname);
        }

        // UserRepository API tests

        [Fact]
        public async Task GetByCredentialsAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var hashedPassword = EncryptionUtil.Encrypt(password);

            var newUser = new User
            {
                Username = username,
                Password = hashedPassword,
                Firstname = "FirstTest",
                Lastname = "LastTest",
                Email = "userTest@example.com"
            };

            await _userRepository.AddAsync(newUser);
            await _fixture.GetContext().SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByCredentialsAsync(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }


        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnCorrectUser()
        {
            // Arrange
            var newUser = new User
            {
                Username = "uniqueUsername",
                Password = "password",
                Firstname = "FirstUnique",
                Lastname = "LastUnique",
                Email = "uniqueusername@example.com",
                Phonenumber = "1234567890",
                Role = UserRole.Citizen
            };

            await _userRepository.AddAsync(newUser);
            await _fixture.GetContext().SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByUsernameAsync("uniqueUsername");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("uniqueUsername", result.Username);
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnCorrectUser()
        {
            // Arrange

            var email = "unique_email@example.com";

            var newUser = new User
            {
                Username = "username",
                Password = "password",
                Firstname = "First",
                Lastname = "Last",
                Email = email,
                Phonenumber = "1234567890",
                Role = UserRole.Citizen
            };

            await _userRepository.AddAsync(newUser);
            await _fixture.GetContext().SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task GetByLastnameAsync_ShouldReturnCorrectUsers()
        {
            // Arrange
            var usersWithLastname = new List<User>
            {
                new User
                {
                    Username = "userWithLastname1",
                    Password = "password1",
                    Firstname = "FirstWithLastname1",
                    Lastname = "CommonLastname",
                    Email = "userwithlastname1@example.com",
                    Phonenumber = "1234567890",
                    Role = UserRole.Citizen
                },
                new User
                {
                    Username = "userWithLastname2",
                    Password = "password2",
                    Firstname = "FirstWithLastname2",
                    Lastname = "CommonLastname",
                    Email = "userwithlastname2@example.com",
                    Phonenumber = "1234567891",
                    Role = UserRole.Citizen
                }
             };

            await _userRepository.AddRangeAsync(usersWithLastname);
            await _fixture.GetContext().SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByLastnameAsync("CommonLastname");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, u => Assert.Equal("CommonLastname", u.Lastname));
        }

        [Fact]
        public async Task GetByRole_ShouldReturnCorrectUsers()
        {
            // Arrange
            var usersWithRole = new List<User>
            {
                new User
                {
                    Username = "userWithRole1",
                    Password = "password1",
                    Firstname = "FirstWithRole1",
                    Lastname = "LastWithRole1",
                    Email = "userwithrole1@example.com",
                    Phonenumber = "1234567890",
                    Role = UserRole.Officer
                },
                new User
                {
                    Username = "userWithRole2",
                    Password = "password2",
                    Firstname = "FirstWithRole2",
                    Lastname = "LastWithRole2",
                    Email = "userwithrole2@example.com",
                    Phonenumber = "1234567891",
                    Role = UserRole.Citizen
                }
            };

            await _userRepository.AddRangeAsync(usersWithRole);
            await _fixture.GetContext().SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByRoleAsync(UserRole.Officer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, u => Assert.Equal(UserRole.Officer, u.Role));
        }

        [Fact]
        public async Task UpdateUserRoleAsync_ShouldUpdateUserRole()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await _userRepository.UpdateUserRoleAsync(userId, UserRole.Officer);
            var updatedUser = await _fixture.Context.Users.FindAsync(userId);

            // Assert
            Assert.True(result);
            Assert.NotNull(updatedUser);
            Assert.Equal(UserRole.Officer, updatedUser.Role);
        }
    }
}