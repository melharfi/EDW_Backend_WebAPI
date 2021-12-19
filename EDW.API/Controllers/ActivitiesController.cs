using EDW.Application.Commands;
using EDW.Application.Exceptions;
using EDW.Application.Queries;
using EDW.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDW.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivitiesController : Controller
    {
        private readonly IMediator mediator;

        public ActivitiesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        #region Gets
        [HttpGet(Name = nameof(GetAllActivitiesAsync))]
        [SwaggerOperation(Summary = "Get All Activities", Description = "Get names of all Activities")]
        //[AllowAnonymous]
        public async Task<List<Activity>> GetAllActivitiesAsync()
        {
            return await mediator.Send(new GetAllActivitiesQuery());
        }
        #endregion

        #region Post
        [HttpPost(Name = nameof(PostActivityAsync))]
        [SwaggerOperation(Summary = "Post Activity", Description = "Post new instance of Activity")]
        public async Task<ActionResult<Guid>> PostActivityAsync([FromQuery] string name, [FromQuery] string code)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    ModelState.AddModelError("Name", "NullOrEmpty");

                if (string.IsNullOrEmpty(code))
                    ModelState.AddModelError("Code", "NullOrEmpty");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                Guid id = await mediator.Send(new CreateActivityQuery(name, code));
                return Ok(id);
            }
            catch (ActivityNameDuplicationException)
            {
                ModelState.AddModelError("Name", "DUPLICATION");
                return BadRequest(ModelState);
            }
            catch (ActivityCodeDuplicationException)
            {
                ModelState.AddModelError("Code", "DUPLICATION");
                return BadRequest(ModelState);
            }
            catch
            {
                ModelState.AddModelError("Error", "InternalError");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Put
        [HttpPut(Name = nameof(PutActivityAsync))]
        [SwaggerOperation(Summary = "Post Activity", Description = "Post new instance of Activity")]
        public async Task<ActionResult> PutActivityAsync([FromQuery] Guid id, [FromQuery] string name, [FromQuery] string code)
        {
            try
            {
                if (id == Guid.Empty)
                    ModelState.AddModelError("Id", "Empty");

                if (string.IsNullOrEmpty(name))
                    ModelState.AddModelError("Name", "NullOrEmpty");

                if (string.IsNullOrEmpty(code))
                    ModelState.AddModelError("Code", "NullOrEmpty");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await mediator.Send(new UpdateActivityQuery(id, name, code));
                return Ok();
            }
            catch (ActivityNotFoundException)
            {
                return NotFound(id);
            }
            catch (ActivityNameDuplicationException)
            {
                ModelState.AddModelError("Name", "DUPLICATION");
                return BadRequest(ModelState);
            }
            catch (ActivityCodeDuplicationException)
            {
                ModelState.AddModelError("Code", "DUPLICATION");
                return BadRequest(ModelState);
            }
            catch
            {
                ModelState.AddModelError("Error", "InternalError");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region Patch

        #endregion

        #region Delete
        [HttpDelete(Name = nameof(DeleteActivityAsync))]
        [SwaggerOperation(Summary = "Delete Activity", Description = "Delete new instance of Activity")]
        public async Task<ActionResult> DeleteActivityAsync([FromQuery] Guid id)
        {
            try
            {
                await mediator.Send(new DeleteActivityQuery(id));
                return Ok();
            }
            catch (ActivityNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                ModelState.AddModelError("Error", "InternalError");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
