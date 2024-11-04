using Transpiler.Common;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.SqlServer.SyntaxProviders;

public class SqlServerFieldSyntaxProvider : IFieldSyntaxProvider
{
    public Dialect Dialect => Dialect.SqlServer;
    public string Field(object? fieldName) => $"\"{fieldName}\"";
}