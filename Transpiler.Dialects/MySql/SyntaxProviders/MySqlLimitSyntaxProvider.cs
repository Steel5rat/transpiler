using Transpiler.Engine.Common;
using Transpiler.Engine.Limit;

namespace Transpiler.Dialects.MySql.SyntaxProviders;

public class MySqlLimitSyntaxProvider : ILimitSyntaxProvider
{
    public Dialect Dialect => Dialect.MySql;
    public string GetLimit(long value) => $"LIMIT {value}";
}