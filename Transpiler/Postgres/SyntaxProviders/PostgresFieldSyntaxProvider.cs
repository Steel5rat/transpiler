using Transpiler.Common;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Postgres.SyntaxProviders;

public class PostgresFieldSyntaxProvider : IFieldSyntaxProvider
{
    public Dialect Dialect => Dialect.Postgres;
    public string Field(object? fieldName) => $"\"{fieldName}\"";
}