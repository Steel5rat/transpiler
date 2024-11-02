using Transpiler.Limit;

namespace Transpiler.SqlServer.SyntaxProviders;

public class SqlServerLimitSyntaxProvider : ILimitSyntaxProvider
{
    public string GetLimit(long value) => $"TOP {value}";
}