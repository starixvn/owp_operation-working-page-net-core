using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.TaskList.Handlers;
using OWP.TaskList.Models;
namespace OWP.TaskList
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [EnableCors("AllowOrigin")]
    public class TaskListController : ControllerBase
    {
        private readonly IMediator mediator;

        public TaskListController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("task-list-table")]
        public async Task<TaskListModel> TaskListTable(TaskListTableRequest request)
            => await mediator.Send(request);
    }
}
