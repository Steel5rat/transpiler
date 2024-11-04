using Transpiler.Common;
using Transpiler.Limit;

namespace Transpiler.MySql.SyntaxProviders;

public class MySqlLimitSyntaxProvider : ILimitSyntaxProvider
{
    public Dialect Dialect => Dialect.MySql;
    public string GetLimit(long value) => $"LIMIT {value}";
}