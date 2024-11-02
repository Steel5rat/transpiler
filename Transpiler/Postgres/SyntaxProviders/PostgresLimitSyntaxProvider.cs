using Transpiler.Limit;

namespace Transpiler.Postgres.SyntaxProviders;

public class PostgresLimitSyntaxProvider : ILimitSyntaxProvider
{
    public string GetLimit(long value) => $"LIMIT {value}";
}