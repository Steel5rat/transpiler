namespace Transpiler.Engine.Where.PhraseGenerators.Empty;

public class IsNotEmptyPhraseGenerator : IPhraseGenerator
{
    private readonly IPhraseGenerator _operand;

    public IsNotEmptyPhraseGenerator(IPhraseGenerator operand)
    {
        _operand = operand;
    }

    public string GetSql() => $"{_operand.GetSql()} IS NOT NULL";
}