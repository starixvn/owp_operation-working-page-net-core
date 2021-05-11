using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.ProductAssignmentReference.Models;
using OWP.ProductAssignmentReference.Services;

namespace OWP.ProductAssignmentReference
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    //[Authorize]
    [AllowAnonymous]
    public class ProductAssignmentController : ControllerBase
    {
        private readonly ProductAssignmentService service;

        public ProductAssignmentController(ProductAssignmentService service)
        {
            this.service = service;
        }

        [HttpGet("ref-resource/{name}")]
        public async Task<IEnumerable<object>> GetRefResourceByName(string name)
        {
            var resourceInfo = (await service.GetResourceByName(name)).ToList();
            resourceInfo.Add(new ReferenceResource
            {
                ResourceId = 0,
                Value = ""
            });
            resourceInfo.Insert(0, new ReferenceResource
            {
                ResourceId= -1,
                Value= "All"
            });
           
            return resourceInfo.Select(r => new
            {
                r.ResourceId,
                r.Value
            });
        }

        [HttpGet]
        public async Task<IEnumerable<UserInfo>> Get([FromQuery] QueryUserInfo userInfo)
            => await service.QueryUser(userInfo);
    }
}
