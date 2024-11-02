namespace Transpiler.Where.SyntaxProviders;

public interface IBinaryLogicSyntaxProvider
{
    string Operator(string operand1, string operand2);
}