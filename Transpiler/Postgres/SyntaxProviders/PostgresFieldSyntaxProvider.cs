using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Postgres.SyntaxProviders;

public class PostgresFieldSyntaxProvider : IFieldSyntaxProvider
{
    public string Field(object? fieldName) => $"\"{fieldName}\"";
}