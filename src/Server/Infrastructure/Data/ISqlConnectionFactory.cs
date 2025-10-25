using System.Data;

namespace BlazorAuthSample.Server.Infrastructure.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
