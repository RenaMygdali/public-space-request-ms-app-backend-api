using PublicSpaceMaintenanceRequestMS.Services.Exceptions;
using System.Net;
using System.Text.Json;

namespace PublicSpaceMaintenanceRequestMS.Helper
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            } catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = exception switch
                {
                    InvalidRegistrationException or
                    InvalidUpdateException or
                    InvalidAssignException or
                    DepartmentCreationException or
                    DepartmentAlreadyExistsException or
                    UserAlreadyExistsException => (int)HttpStatusCode.BadRequest,        //400

                    ForbiddenException or => (int)HttpStatusCode.Forbidden,              //403

                    UserNotFoundException or
                    OfficerNotFoundException or
                    DepartmentNotFoundException or
                    RequestNotFoundException=> (int)HttpStatusCode.NotFound,             //404

                    EmailAlreadyExistsException or
                    UsernameAlreadyExistsException => (int)HttpStatusCode.Conflict,      //409

                    _ => (int)HttpStatusCode.InternalServerError                         //500
                };

                var result = JsonSerializer.Serialize(new { message = exception?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
