using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Postgres.SyntaxProviders;

public class RedshiftFieldSyntaxProvider : IFieldSyntaxProvider
{
    public string Field(object? fieldName) => $"<<{fieldName}>>"; //just for demo purposes
}