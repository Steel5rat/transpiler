using Transpiler.Engine.Common;

namespace Transpiler.Engine.Limit;

public interface ILimitSyntaxProvider
{
    Dialect Dialect { get; }
    string GetLimit(long value);
}