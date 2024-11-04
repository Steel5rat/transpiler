using Transpiler.Common;

namespace Transpiler.Where.SyntaxProviders;

public interface IFieldSyntaxProvider
{
    Dialect Dialect { get; }
    string Field(object? fieldName);
}