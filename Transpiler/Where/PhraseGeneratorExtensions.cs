using Transpiler.Where.PhraseGenerators;

namespace Transpiler.Where;

public static class PhraseGeneratorExtensions
{
    public static bool IsNull(this IPhraseGenerator phraseGenerator)
    {
        return phraseGenerator is NullPhraseGeneratorFactory.NullPhraseGenerator;
    }
    
    public static bool IsField(this IPhraseGenerator phraseGenerator)
    {
        return phraseGenerator is FieldPhraseGeneratorFactory.FieldPhraseGenerator;
    }

    public static bool IsNumber(this IPhraseGenerator phraseGenerator)
    {
        return phraseGenerator is NumberPhraseGeneratorFactory.NumberPhraseGenerator;
    }

    public static bool IsString(this IPhraseGenerator phraseGenerator)
    {
        return phraseGenerator is StringPhraseGeneratorFactory.StringPhraseGenerator;
    }
    
    public static bool IsBool(this IPhraseGenerator phraseGenerator)
    {
        return phraseGenerator is BoolPhraseGeneratorFactory.BoolPhraseGenerator;
    }
    
    public static bool IsPrimitive(this IPhraseGenerator phraseGenerator)
    {
        return phraseGenerator.IsNull() || phraseGenerator.IsNumber() || phraseGenerator.IsString() || phraseGenerator.IsBool();
    }
    
}