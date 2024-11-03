namespace Transpiler.Where.PhraseGenerators.NotPhrase;

public class NotPhraseGenerator : IPhraseGenerator
{
    public NotPhraseGenerator(IPhraseGenerator operand)
    {
        Operand = operand;
    }

    public IPhraseGenerator Operand { get; }

    public string GetSql()
    {
        return $"NOT ({Operand.GetSql()})";
    }
}