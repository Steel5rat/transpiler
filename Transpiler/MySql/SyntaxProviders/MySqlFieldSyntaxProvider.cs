using Transpiler.Where.SyntaxProviders;

namespace Transpiler.MySql.SyntaxProviders;

public class MySqlFieldSyntaxProvider : IFieldSyntaxProvider
{
    public string Field(object? fieldName) => $"`{fieldName}`";
}