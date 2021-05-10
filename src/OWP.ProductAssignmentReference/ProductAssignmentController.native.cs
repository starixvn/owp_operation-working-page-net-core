using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.ProductAssignmentReference.Models;

namespace OWP.ProductAssignmentReference
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [Authorize]
    public class ProductAssignmentController : ControllerBase
    {

        public async Task<IEnumerable<UserInfo>> QueryUserInfo([FromUri]UserInfo userInfo)
        {
            return null;
        }
    }
}
