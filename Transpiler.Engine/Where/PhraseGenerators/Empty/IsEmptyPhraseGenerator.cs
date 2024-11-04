namespace Transpiler.Engine.Where.PhraseGenerators.Empty;

public class IsEmptyPhraseGenerator : IPhraseGenerator
{
    private readonly IPhraseGenerator _operand;

    public IsEmptyPhraseGenerator(IPhraseGenerator operand)
    {
        _operand = operand;
    }

    public string GetSql() => $"{_operand.GetSql()} IS NULL";
}