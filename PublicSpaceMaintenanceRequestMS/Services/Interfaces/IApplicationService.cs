namespace PublicSpaceMaintenanceRequestMS.Services.Interfaces
{
    public interface IApplicationService
    {
        UserService UserService { get; }
        CitizenService CitizenService { get; }
        OfficerService OfficerService { get; }
        //AdminService AdminService { get; }
        RequestService RequestService { get; }
        DepartmentService DepartmentService { get; }
    }
}
