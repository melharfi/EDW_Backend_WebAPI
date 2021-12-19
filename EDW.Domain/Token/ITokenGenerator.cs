using EDW.Domain.Models;

namespace EDW.Domain
{
    public interface ITokenGenerator
    {
        string Generate(User user);
    }
}
