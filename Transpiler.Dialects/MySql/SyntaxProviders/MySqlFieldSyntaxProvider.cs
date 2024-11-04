using Transpiler.Engine.Common;
using Transpiler.Engine.Where.SyntaxProviders;

namespace Transpiler.Dialects.MySql.SyntaxProviders;

public class MySqlFieldSyntaxProvider : IFieldSyntaxProvider
{
    public Dialect Dialect => Dialect.MySql;
    public string Field(object? fieldName) => $"`{fieldName}`";
}