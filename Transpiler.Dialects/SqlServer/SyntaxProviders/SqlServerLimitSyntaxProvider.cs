using Transpiler.Engine.Common;
using Transpiler.Engine.Limit;

namespace Transpiler.Dialects.SqlServer.SyntaxProviders;

public class SqlServerLimitSyntaxProvider : ILimitSyntaxProvider
{
    public Dialect Dialect => Dialect.SqlServer;
    public string GetLimit(long value) => $"TOP {value}";
}