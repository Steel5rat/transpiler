namespace Transpiler.Where.PhraseGenerators.Bool;

public class BoolPhraseGenerator : IPhraseGenerator
{
    private readonly bool _value;

    public BoolPhraseGenerator(bool value)
    {
        _value = value;
    }

    public string GetSql()
    {
        return _value ? "TRUE" : "FALSE";
    }
}