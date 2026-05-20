using WebProjekat.Common;

namespace UserService.Interfaces
{
    public interface IJwtService
    {
        Result<string> GenerateToken(Guid userId, string email, string role);

    }
}
