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
            return resourceInfo.Select(r => new
            {
                r.ResourceId,
                r.Value
            });
        }

        [HttpGet("last-update")]
        public async Task<object> GetLastUpdate()
            => await service.GetLastUpdate();

        [HttpGet]
        public async Task<IEnumerable<UserInfo>> Get([FromQuery] QueryUserInfo queryInfo)
            => await service.QueryUser(queryInfo);

        [HttpPost]
        public async Task Post(UserInfoEditor info)
            => await service.UpdateUserInfo(info);
    }
}
