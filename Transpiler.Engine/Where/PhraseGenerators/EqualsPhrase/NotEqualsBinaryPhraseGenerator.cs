﻿using Transpiler.Engine.Where.PhraseGenerators.Null;
using Transpiler.Engine.Where.SyntaxProviders;

namespace Transpiler.Engine.Where.PhraseGenerators.EqualsPhrase;

public class NotEqualsBinaryPhraseGenerator : IPhraseGenerator
{
    private readonly IEqualsSyntaxProvider _syntaxProvider;
    private readonly IPhraseGenerator _operand1;
    private readonly IPhraseGenerator _operand2;

    public NotEqualsBinaryPhraseGenerator(IEqualsSyntaxProvider syntaxProvider, IPhraseGenerator operand1,
        IPhraseGenerator operand2)
    {
        _syntaxProvider = syntaxProvider;
        _operand1 = operand1;
        _operand2 = operand2;
    }

    public string GetSql()
    {
        return _operand2 is NullPhraseGenerator
            ? _syntaxProvider.IsNotNull(_operand1.GetSql())
            : _operand1 is NullPhraseGenerator
                ? _syntaxProvider.IsNotNull(_operand2.GetSql())
                : _syntaxProvider.AreNotEqual(_operand1.GetSql(), _operand2.GetSql());
    }
}