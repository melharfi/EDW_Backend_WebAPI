using EDW.Application.Models;
using EDW.Domain;
using EDW.Domain.Models;
using EDW.Infrastructure;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EDW.Application.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResult>
    {
        private readonly AppDbContext context;
        private readonly ITokenGenerator tokenGenerator;
        public LoginQueryHandler(AppDbContext context, ITokenGenerator tokenGenerator)
        {
            this.tokenGenerator = tokenGenerator;
            this.context = context;
        }
        public Task<LoginResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Task.FromResult(LoginResult.Fail());

            // add credentials checks with database
            User result = context.Users.FirstOrDefault(f => f.Username == request.Username && f.Password == request.Password);

            if(result == null)
            {
                return Task.FromResult(LoginResult.Fail());
            }
            else
            {
                // update activity status
                User user = context.Users.FirstOrDefault(f => f.Username == request.Username);
                user.ActivityCode = "";
                user.LastModifiedActivityTimeStamp = 0;

                context.SaveChanges();

                //create Token
                string token = tokenGenerator.Generate(result);
                return Task.FromResult(LoginResult.Success(token, result.FirstName, result.LastName));
            }            
        }
    }
}
