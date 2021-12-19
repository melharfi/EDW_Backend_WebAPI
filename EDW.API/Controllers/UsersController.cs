using EDW.Application.Commands;
using EDW.Application.Queries;
using EDW.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using EDW.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using EDW.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace EDW.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IMediator mediator;
        private readonly IHubContext<ActivityHub, IActivityClient> activityHubContext;

        public UsersController(IMediator mediator, IHubContext<ActivityHub, IActivityClient> activityHubContext)
        {
            this.mediator = mediator;
            this.activityHubContext = activityHubContext;
        }

        #region Gets
        [HttpGet(Name = nameof(GetAvailableUsersAsync))]
        [SwaggerOperation(Summary = "Get available Users", Description = "Get Activity status of available user logged and has a status")]
        public async Task<List<User>> GetAvailableUsersAsync()
        {
            return await mediator.Send(new GetAvailableUsersQuery());
        }
        #endregion

        #region Post
        //[HttpPost(Name = nameof(PostUserAsync))]
        //[SwaggerOperation(Summary = "Post User", Description = "Post new instance of User")]
        //public async Task<ActionResult<Guid>> PostUserAsync([FromQuery] string name, [FromQuery] string code)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(name))
        //            ModelState.AddModelError("Name", "NullOrEmpty");

        //        if (string.IsNullOrEmpty(code))
        //            ModelState.AddModelError("Code", "NullOrEmpty");

        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);
                
        //        Guid id = await mediator.Send(new CreateUserQuery(name, code));
        //        return Ok(id);
        //    }
        //    catch (UserNameDuplicationException)
        //    {
        //        ModelState.AddModelError("Name", "DUPLICATION");
        //        return BadRequest(ModelState);
        //    }
        //    catch (UserCodeDuplicationException)
        //    {
        //        ModelState.AddModelError("Code", "DUPLICATION");
        //        return BadRequest(ModelState);
        //    }
        //    catch
        //    {
        //        ModelState.AddModelError("Error", "InternalError");
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
        #endregion

        #region Put
        //[HttpPut(Name = nameof(PutUserAsync))]
        //[SwaggerOperation(Summary = "Post User", Description = "Post new instance of User")]
        //public async Task<ActionResult> PutUserAsync([FromQuery] Guid id, [FromQuery] string name, [FromQuery] string code)
        //{
        //    try
        //    {
        //        if (id == Guid.Empty)
        //            ModelState.AddModelError("Id", "Empty");

        //        if (string.IsNullOrEmpty(name))
        //            ModelState.AddModelError("Name", "NullOrEmpty");

        //        if (string.IsNullOrEmpty(code))
        //            ModelState.AddModelError("Code", "NullOrEmpty");

        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);

        //        await mediator.Send(new UpdateUserQuery(id, name, code));
        //        return Ok();
        //    }
        //    catch (UserNotFoundException)
        //    {
        //        return NotFound(id);
        //    }
        //    catch (UserNameDuplicationException)
        //    {
        //        ModelState.AddModelError("Name", "DUPLICATION");
        //        return BadRequest(ModelState);
        //    }
        //    catch (UserCodeDuplicationException)
        //    {
        //        ModelState.AddModelError("Code", "DUPLICATION");
        //        return BadRequest(ModelState);
        //    }
        //    catch
        //    {
        //        ModelState.AddModelError("Error", "InternalError");
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //[HttpPatch(Name = nameof(PatchUserUserAsync))]
        //[SwaggerOperation(Summary = "Patch User User", Description = "Patch current user User")]
        //public async Task<ActionResult> PatchUserUserAsync([FromQuery] string newCode)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(newCode))
        //            ModelState.AddModelError("User", "UNKNOWN");

        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);

        //        Console.WriteLine(newCode);
        //        return Ok();
        //    }
        //    catch
        //    {
        //        ModelState.AddModelError("Error", "InternalError");
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
        #endregion

        #region Patch
        [HttpPatch(Name = nameof(PatchUserActivityAsync))]
        [SwaggerOperation(Summary = "Patch current user ActivityCode", Description = "Modify user activity Code")]
        public async Task<ActionResult> PatchUserActivityAsync([FromQuery] string newActivityCode)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var username = claimsIdentity.FindFirst("Username")?.Value;

                await mediator.Send(new UpdateUserActivityQuery(username, newActivityCode));

                #region calling SignalR notification
                await activityHubContext.Clients.All.ActivityChanged();
                #endregion

                return Ok();
            }
            catch (ActivityNotFoundException)
            {
                return NotFound(newActivityCode);
            }
            catch
            {
                ModelState.AddModelError("Error", "InternalError");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Delete
        //[HttpDelete(Name = nameof(DeleteUserAsync))]
        //[SwaggerOperation(Summary = "Delete User", Description = "Delete new instance of User")]
        //public async Task<ActionResult> DeleteUserAsync([FromQuery] Guid id)
        //{
        //    try
        //    {
        //        await mediator.Send(new DeleteUserQuery(id));
        //        return Ok();
        //    }
        //    catch (UserNotFoundException)
        //    {
        //        return NotFound();
        //    }
        //    catch
        //    {
        //        ModelState.AddModelError("Error", "InternalError");
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
        #endregion
    }
}
