using Transpiler.Limit;

namespace Transpiler.MySql.SyntaxProviders;

public class MySqlLimitSyntaxProvider : ILimitSyntaxProvider
{
    public string GetLimit(long value) => $"LIMIT {value}";
}