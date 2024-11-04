using Transpiler.Engine.Common;
using Transpiler.Engine.Where.SyntaxProviders;

namespace Transpiler.Dialects.SqlServer.SyntaxProviders;

public class SqlServerFieldSyntaxProvider : IFieldSyntaxProvider
{
    public Dialect Dialect => Dialect.SqlServer;
    public string Field(object? fieldName) => $"\"{fieldName}\"";
}