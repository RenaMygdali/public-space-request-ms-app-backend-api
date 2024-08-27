using PublicSpaceMaintenanceRequestMS.Data;
using PublicSpaceMaintenanceRequestMS.Repositories;
using PublicSpaceMaintenanceRequestMS.Tests.Fixtures;

namespace PublicSpaceMaintenanceRequestMS.Tests.Repositories
{
    public class DepartmentRepositoryTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly TestDatabaseFixture _fixture;
        private readonly DepartmentRepository _departmentRepository;

        public DepartmentRepositoryTests(TestDatabaseFixture fixture)
        {
            _fixture = fixture;

            _departmentRepository = new DepartmentRepository(_fixture.GetContext());
        }

        [Fact]
        public async Task GetByTitleAsync_ShouldReturnDepartmentWithTitle()
        {
            // Arrange
            var departmentTitle = "Department 1";

            // Act
            var result = await _departmentRepository.GetByTitleAsync(departmentTitle);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(departmentTitle, result.Title);
        }

        [Fact]
        public async Task GetByTitleAsync_ShouldReturnNull_WhenDepartmentNotFound()
        {
            // Arrange
            var nonExistentTitle = "Non-Existent Department";

            // Act
            var result = await _departmentRepository.GetByTitleAsync(nonExistentTitle);

            // Assert
            Assert.Null(result);
        }
    }
}
