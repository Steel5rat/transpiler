using Transpiler.Common;

namespace Transpiler.Where.SyntaxProviders;

public interface IEqualsSyntaxProvider
{
    Dialect Dialect { get; }
    
    string AreEqual(string operand1, string operand2);
    string AreNotEqual(string operand1, string operand2);
    string AreEqual(string operand1, IEnumerable<string> operand2);
    string AreNotEqual(string operand1, IEnumerable<string> operand2);
    string IsNull(string operand);
    string IsNotNull(string operand);
}