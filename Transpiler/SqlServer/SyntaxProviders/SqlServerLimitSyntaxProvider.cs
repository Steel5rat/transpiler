using Transpiler.Common;
using Transpiler.Limit;

namespace Transpiler.SqlServer.SyntaxProviders;

public class SqlServerLimitSyntaxProvider : ILimitSyntaxProvider
{
    public Dialect Dialect => Dialect.SqlServer;
    public string GetLimit(long value) => $"TOP {value}";
}