using Transpiler.Engine.Common;
using Transpiler.Engine.Where.SyntaxProviders;

namespace Transpiler.Dialects.Redshift.SyntaxProviders;

public class RedshiftFieldSyntaxProvider : IFieldSyntaxProvider
{
    public Dialect Dialect => Dialect.Redshift;
    public string Field(object? fieldName) => $"<<{fieldName}>>"; //just for demo purposes
}