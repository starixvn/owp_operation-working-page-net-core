namespace OWP.Event.LiveGameAssignment.User;

using Constants;
using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/event-assignment/user")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserRepository repository;

    public UserController(UserRepository repository)
    {
        this.repository = repository;
    }

    [Route("")]
    [HttpGet]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Read)]
    public async Task<IEnumerable<UserInfo>> Query(DateTime eventDate)
    {
        return await repository.Query(eventDate);
    }
}