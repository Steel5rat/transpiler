using Transpiler.Limit;

namespace Transpiler.Postgres.SyntaxProviders;

public class RedshiftLimitSyntaxProvider : ILimitSyntaxProvider
{
    public string GetLimit(long value) => $"MAX LIMIT {value}"; // just for demo purposes
}