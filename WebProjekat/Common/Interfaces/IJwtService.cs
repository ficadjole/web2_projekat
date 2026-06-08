using WebProjekat.Common;

namespace Common.Interfaces
{
    public interface IJwtService
    {
        Result<string> GenerateToken(Guid userId, string email, string role,string name);

    }
}
