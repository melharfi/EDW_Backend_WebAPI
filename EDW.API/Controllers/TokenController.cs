using EDW.API.DTOs;
using EDW.API.Encryption;
using EDW.Application.Models;
using EDW.Application.Queries;
using EDW.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace EDW.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IEncrypt encrypt;

        public TokenController(IMediator mediator, IEncrypt encrypt)
        {
            this.mediator = mediator;
            this.encrypt = encrypt;
        }

        [AllowAnonymous]
        [HttpPost(Name = nameof(LoginAsync))]
        [SwaggerOperation(Summary = "Authentication", Description = "Authenticate and return a Token using LoginResult")]
        public async Task<ActionResult<LoginResult>> LoginAsync([FromBody] LoginDto dto, CancellationToken cancellationToken = default)
        {
            if (null == dto)
                return BadRequest();
            var command = new LoginQuery(dto.Username, encrypt.HashPassword(dto.Password));
            LoginResult token = await mediator.Send(command, cancellationToken);

            return token;
        }
    }
}
