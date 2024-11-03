using System.Collections.Immutable;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators.EqualsPhrase;

public class NotEqualsPolyPhraseGenerator : IPhraseGenerator
{
    private readonly IEqualsSyntaxProvider _syntaxProvider;
    private readonly ImmutableList<IPhraseGenerator> _operands;

    public NotEqualsPolyPhraseGenerator(IEqualsSyntaxProvider syntaxProvider, ImmutableList<IPhraseGenerator> operands)
    {
        _syntaxProvider = syntaxProvider;
        _operands = operands;
    }
    public string GetSql()
    {
        return _syntaxProvider.AreNotEqual(_operands[0].GetSql(), _operands.Skip(1).Select(o => o.GetSql()));
    }
}