using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;

namespace PublicSpaceMaintenanceRequestMS.Tests.Fixtures
{
    public class TestDatabaseFixture : IDisposable
    {
        public PublicSpaceMaintenanceRequestDbDBContext Context { get; private set; }

        public TestDatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<PublicSpaceMaintenanceRequestDbDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .EnableSensitiveDataLogging()
                .Options;

            Context = new PublicSpaceMaintenanceRequestDbDBContext(options);

            SeedData();
        }

        public void SeedData()
        {
            // Προσθήκη dummy δεδομένων
            var departments = new List<Department>
            {
                new Department { Id = 1, Title = "Department 1" },
                new Department { Id = 2, Title = "Department 2" }
            };

                var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Password = "password123", Firstname = "First1", Lastname = "Last1", Email = "user1@example.com", Phonenumber = "1234567890", Role = UserRole.Citizen },
                new User { Id = 2, Username = "user2", Password = "password123", Firstname = "First2", Lastname = "Last2", Email = "user2@example.com", Phonenumber = "1234567890", Role = UserRole.Officer }
            };

                var requests = new List<Request>
            {
                new Request { Id = 1, Status = RequestStatus.Pending, CitizenId = 1, AssignedDepartmentId = 2 },
                new Request { Id = 2, Status = RequestStatus.Completed, CitizenId = 1, AssignedDepartmentId = 1 },
                new Request { Id = 3, Status = RequestStatus.Pending, CitizenId = 2, AssignedDepartmentId = 2 }
            };

            Context.Departments.AddRange(departments);
            Context.Users.AddRange(users);
            Context.Requests.AddRange(requests);

            Context.SaveChanges();
        }

        public PublicSpaceMaintenanceRequestDbDBContext GetContext()
        {
            return Context;
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
