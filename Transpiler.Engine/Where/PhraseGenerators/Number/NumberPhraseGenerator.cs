using System.Globalization;

namespace Transpiler.Engine.Where.PhraseGenerators.Number;

public class NumberPhraseGenerator : IPhraseGenerator
{
    private readonly decimal _value;

    public NumberPhraseGenerator(decimal value)
    {
        _value = value;
    }
    public string GetSql()
    {
        return _value.ToString(CultureInfo.InvariantCulture);
    }
}