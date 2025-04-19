using Core.Entities;

namespace Core.Services;

public interface IJwtTokenService
{
    string CreateToken(User user);
}
