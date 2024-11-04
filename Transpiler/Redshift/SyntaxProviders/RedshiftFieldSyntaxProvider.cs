using Transpiler.Common;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Postgres.SyntaxProviders;

public class RedshiftFieldSyntaxProvider : IFieldSyntaxProvider
{
    public Dialect Dialect => Dialect.Redshift;
    public string Field(object? fieldName) => $"<<{fieldName}>>"; //just for demo purposes
}