namespace Transpiler.Engine.Where.PhraseGenerators.StringPhrase;

public class StringPhraseGenerator : IPhraseGenerator
{
    private readonly string _value;

    public StringPhraseGenerator(string value)
    {
        _value = value;
    }

    public string GetSql()
    {
        return $"'{_value}'";
    }
}