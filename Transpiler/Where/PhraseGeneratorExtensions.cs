using Transpiler.Where.PhraseGenerators;
using Transpiler.Where.PhraseGenerators.Bool;
using Transpiler.Where.PhraseGenerators.Null;
using Transpiler.Where.PhraseGenerators.Number;
using Transpiler.Where.PhraseGenerators.StringPhrase;

namespace Transpiler.Where;

public static class PhraseGeneratorExtensions
{
    public static bool IsPrimitive(this IPhraseGenerator phraseGenerator)
    {
        return phraseGenerator is NullPhraseGenerator or NumberPhraseGenerator or StringPhraseGenerator or BoolPhraseGenerator;
    }
}