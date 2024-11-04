using Transpiler.Engine.Common;
using Transpiler.Engine.Limit;

namespace Transpiler.Dialects.Postgres.SyntaxProviders;

public class PostgresLimitSyntaxProvider : ILimitSyntaxProvider
{
    public Dialect Dialect => Dialect.Postgres;
    public string GetLimit(long value) => $"LIMIT {value}";
}