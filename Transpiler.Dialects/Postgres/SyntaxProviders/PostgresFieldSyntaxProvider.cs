using Transpiler.Engine.Common;
using Transpiler.Engine.Where.SyntaxProviders;

namespace Transpiler.Dialects.Postgres.SyntaxProviders;

public class PostgresFieldSyntaxProvider : IFieldSyntaxProvider
{
    public Dialect Dialect => Dialect.Postgres;
    public string Field(object? fieldName) => $"\"{fieldName}\"";
}