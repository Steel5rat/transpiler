using Transpiler.Common;
using Transpiler.Limit;

namespace Transpiler.Postgres.SyntaxProviders;

public class PostgresLimitSyntaxProvider : ILimitSyntaxProvider
{
    public Dialect Dialect => Dialect.Postgres;
    public string GetLimit(long value) => $"LIMIT {value}";
}