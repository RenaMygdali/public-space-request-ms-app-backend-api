using Microsoft.AspNetCore.Mvc;
using PublicSpaceMaintenanceRequestMS.Models;

namespace PublicSpaceMaintenanceRequestMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetRoles()
        {
            // Λήψη των ρόλων από την enum και μετατροπή τους σε λίστα string
            try
            {
                var roles = Enum.GetValues(typeof(UserRole))
                                 .Cast<UserRole>()
                                 .Select(r => r.ToString())
                                 .ToList();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
