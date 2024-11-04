using System.Collections.Immutable;
using Transpiler.Engine.Where.SyntaxProviders;

namespace Transpiler.Engine.Where.PhraseGenerators.EqualsPhrase;

public class EqualsPolyPhraseGenerator : IPhraseGenerator
{
    private readonly IEqualsSyntaxProvider _syntaxProvider;
    private readonly ImmutableList<IPhraseGenerator> _operands;

    public EqualsPolyPhraseGenerator(IEqualsSyntaxProvider syntaxProvider, ImmutableList<IPhraseGenerator> operands)
    {
        _syntaxProvider = syntaxProvider;
        _operands = operands;
    }
    public string GetSql()
    {
        return _syntaxProvider.AreEqual(_operands[0].GetSql(), _operands.Skip(1).Select(o => o.GetSql()));
    }
}