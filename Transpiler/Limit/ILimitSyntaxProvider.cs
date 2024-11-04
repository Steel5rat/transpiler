using Transpiler.Common;

namespace Transpiler.Limit;

public interface ILimitSyntaxProvider
{
    Dialect Dialect { get; }
    string GetLimit(long value);
}