using Microsoft.AspNetCore.Mvc;

namespace CieBackendAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {
    }
}
