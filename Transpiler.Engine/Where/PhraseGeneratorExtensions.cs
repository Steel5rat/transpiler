using Transpiler.Engine.Where.PhraseGenerators;
using Transpiler.Engine.Where.PhraseGenerators.Bool;
using Transpiler.Engine.Where.PhraseGenerators.Null;
using Transpiler.Engine.Where.PhraseGenerators.Number;
using Transpiler.Engine.Where.PhraseGenerators.StringPhrase;

namespace Transpiler.Engine.Where;

public static class PhraseGeneratorExtensions
{
    public static bool IsPrimitive(this IPhraseGenerator phraseGenerator)
    {
        return phraseGenerator is NullPhraseGenerator or NumberPhraseGenerator or StringPhraseGenerator or BoolPhraseGenerator;
    }
}