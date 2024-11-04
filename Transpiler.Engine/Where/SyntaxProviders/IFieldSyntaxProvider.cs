using Transpiler.Engine.Common;

namespace Transpiler.Engine.Where.SyntaxProviders;

public interface IFieldSyntaxProvider
{
    Dialect Dialect { get; }
    string Field(object? fieldName);
}