using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Models;
using PublicSpaceMaintenanceRequestMS.Repositories;
using PublicSpaceMaintenanceRequestMS.Tests.Fixtures;

namespace PublicSpaceMaintenanceRequestMS.Tests.Repositories
{
    public class RequestRepositoryTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly TestDatabaseFixture _fixture;
        private readonly RequestRepository _requestRepository;

        public RequestRepositoryTests(TestDatabaseFixture fixture)
        {
            _fixture = fixture;

            _requestRepository = new RequestRepository(_fixture.GetContext());
        }


        // RequestRepository API tests

        [Fact]
        public async Task GetByStatusAsync_ShouldReturnRequestsByStatus()
        {
            // Act
            var result = await _requestRepository.GetByStatusAsync(RequestStatus.Pending);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, req => Assert.Equal(RequestStatus.Pending, req.Status));
        }



        [Fact]
        public async Task GetByCitizenIdAsync_ShouldReturnRequestsByCitizenId()
        {
            // Act
            var result = await _requestRepository.GetByCitizenIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.All(result, r => Assert.Equal(1, r.CitizenId));
            Assert.Equal(2, result.Count());
        }



        [Fact]
        public async Task GetByDepartmentAsync_ShouldReturnRequestsByDepartmentId()
        {
            // Act
            var result = await _requestRepository.GetByDepartmentAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, r => Assert.Equal(RequestStatus.Pending, r.Status));
        }



        [Fact]
        public async Task UpdateRequestStatusAsync_ShouldUpdateRequestStatus()
        {
            // Arrange
            var newRequest = new Request
            {
                Description = "RequestDiscription",
                Status = RequestStatus.Pending,
                CitizenId = 3,
                AssignedDepartmentId = 1
            };

            await _requestRepository.AddAsync(newRequest);
            await _fixture.GetContext().SaveChangesAsync();

            // Act
            var result = await _requestRepository.UpdateRequestStatusAsync(newRequest.Id, RequestStatus.Completed);
            var updatedRequest = await _fixture.GetContext().Requests.FindAsync(newRequest.Id);

            // Assert
            Assert.True(result);
            Assert.NotNull(updatedRequest);
            Assert.Equal(RequestStatus.Completed, updatedRequest.Status);
            Assert.Equal(newRequest.Id, updatedRequest.Id);
        }



        [Fact]
        public async Task AssignRequestAsync_ShouldAssignRequestToDepartment()
        {
            // Arrange
            var requestId = 2;
            var newDepartmentId = 3;

            // Act

            var result = await _requestRepository.AssignRequestAsync(requestId, newDepartmentId);
            var assignedRequest = await _fixture.GetContext().Requests.FindAsync(requestId);

            // Assert
            Assert.True(result);
            Assert.NotNull(assignedRequest);
            Assert.Equal(newDepartmentId, assignedRequest.AssignedDepartmentId);
        }
    }
}
