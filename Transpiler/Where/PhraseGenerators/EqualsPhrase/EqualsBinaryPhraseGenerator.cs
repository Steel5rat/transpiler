using Transpiler.Where.PhraseGenerators.Null;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators.EqualsPhrase;

public class EqualsBinaryPhraseGenerator : IPhraseGenerator
{
    private readonly IEqualsSyntaxProvider _syntaxProvider;
    private readonly IPhraseGenerator _operand1;
    private readonly IPhraseGenerator _operand2;

    public EqualsBinaryPhraseGenerator(IEqualsSyntaxProvider syntaxProvider, IPhraseGenerator operand1,
        IPhraseGenerator operand2)
    {
        _syntaxProvider = syntaxProvider;
        _operand1 = operand1;
        _operand2 = operand2;
    }

    public string GetSql()
    {
        return _operand2 is NullPhraseGenerator
            ? _syntaxProvider.IsNull(_operand1.GetSql())
            : _operand1 is NullPhraseGenerator
                ? _syntaxProvider.IsNull(_operand2.GetSql())
                : _syntaxProvider.AreEqual(_operand1.GetSql(), _operand2.GetSql());
    }
}