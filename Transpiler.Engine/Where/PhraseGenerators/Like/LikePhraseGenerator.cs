﻿namespace Transpiler.Engine.Where.PhraseGenerators.Like;

public class LikePhraseGenerator : IPhraseGenerator
{
    private readonly IPhraseGenerator _operand1;
    private readonly IPhraseGenerator _operand2;

    public LikePhraseGenerator(IPhraseGenerator operand1, IPhraseGenerator operand2)
    {
        _operand1 = operand1;
        _operand2 = operand2;
    }

    public string GetSql()
    {
        return $"{_operand1.GetSql()} LIKE {_operand2.GetSql()}";
    }
}