namespace OWP.PreMatchNotification
{
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Cors;
  using Microsoft.AspNetCore.Mvc;


  [Route("api/[controller]")]
  [ApiController]
  [AllowAnonymous]
  [EnableCors("AllowOrigin")]
  public class PrematchNotificationController : ControllerBase
  {
    [HttpGet]
    public int Get() => 5;
  }
}