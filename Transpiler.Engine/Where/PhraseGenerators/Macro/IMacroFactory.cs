using System.Collections.Immutable;

namespace Transpiler.Engine.Where.PhraseGenerators.Macro;

public interface IMacroFactory
{
    IImmutableDictionary<string, Macro> CreateMacros(Dictionary<string, List<object>> macros);
}