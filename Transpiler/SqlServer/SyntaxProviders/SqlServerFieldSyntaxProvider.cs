using Transpiler.Where.SyntaxProviders;

namespace Transpiler.SqlServer.SyntaxProviders;

public class SqlServerFieldSyntaxProvider : IFieldSyntaxProvider
{
    public string Field(object? fieldName) => $"\"{fieldName}\"";
}