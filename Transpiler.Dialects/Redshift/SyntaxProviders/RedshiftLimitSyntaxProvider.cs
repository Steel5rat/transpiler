using Transpiler.Engine.Common;
using Transpiler.Engine.Limit;

namespace Transpiler.Dialects.Redshift.SyntaxProviders;

public class RedshiftLimitSyntaxProvider : ILimitSyntaxProvider
{
    public Dialect Dialect => Dialect.Redshift;
    public string GetLimit(long value) => $"MAX LIMIT {value}"; // just for demo purposes
}