using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Core.IServices;
using Sat.Recruitment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserService userService;
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            this.userService = userService;
            this._logger = logger;
        }

        [HttpPost]
        [Produces(typeof(User))]
        [AllowAnonymous]
        public async Task<IActionResult> Post(User user)
        {
            bool result = await userService.ValidateUserAsync(user).ConfigureAwait(false);
            _logger.LogInformation(message: $"Validated user method was call at {DateTime.UtcNow.ToLongTimeString()}");

            if (result)
            {
                _logger.LogInformation(message: $"User was created sucessfull at {DateTime.UtcNow.ToLongTimeString()}");
                return Ok(new { message = "Succesfull", state = true });
                
            }
            else
            {
                _logger.LogInformation(message: $" Bad request adding new user, please see below {DateTime.UtcNow.ToLongTimeString()}");
                return BadRequest();
            }
        }
    }
}
