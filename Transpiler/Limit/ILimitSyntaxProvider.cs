namespace Transpiler.Limit;

public interface ILimitSyntaxProvider
{
    string GetLimit(long value);
}