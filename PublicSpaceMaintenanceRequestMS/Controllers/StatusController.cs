using Microsoft.AspNetCore.Mvc;
using PublicSpaceMaintenanceRequestMS.Models;

namespace PublicSpaceMaintenanceRequestMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetStatus()
        {
            // Λήψη των request status από την enum και μετατροπή τους σε λίστα string
            try
            {
                var statusList = Enum.GetValues(typeof(RequestStatus))
                                 .Cast<RequestStatus>()
                                 .Select(r => r.ToString())
                                 .ToList();
                return Ok(statusList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
