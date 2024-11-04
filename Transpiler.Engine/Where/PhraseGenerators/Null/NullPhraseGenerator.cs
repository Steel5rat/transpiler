namespace Transpiler.Engine.Where.PhraseGenerators.Null;

public class NullPhraseGenerator : IPhraseGenerator
{
    public string GetSql()
    {
        return "NULL";
    }
}