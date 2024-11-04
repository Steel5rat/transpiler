using System.Collections.Immutable;

namespace Transpiler.Engine.Where.PhraseGenerators.Macro;

public class MacroFactory : IMacroFactory
{
    public IImmutableDictionary<string, Engine.Where.PhraseGenerators.Macro.Macro> CreateMacros(Dictionary<string, List<object>> macros)
    {
        return ValidateCircularDependencies(
            macros.ToDictionary(
                    macro => macro.Key,
                    macro => new Engine.Where.PhraseGenerators.Macro.Macro(macro.Value))
                .ToImmutableDictionary());
    }

    private IImmutableDictionary<string, Engine.Where.PhraseGenerators.Macro.Macro> ValidateCircularDependencies(IImmutableDictionary<string, Engine.Where.PhraseGenerators.Macro.Macro> macros)
    {
        var macrosWithCircularDependencies = macros.Keys.Where(ContainsCircularDependency).ToList();

        if (macrosWithCircularDependencies.Any())
        {
            throw new InvalidOperationException($"Detected circular dependency macros: {string.Join("; ", macrosWithCircularDependencies)}");
        }

        return macros;

        bool ContainsCircularDependency(string rootMacro)
        {
            var macroDefinition = macros[rootMacro].Definition;
            var queue = new Queue<string>(GetMacroDependencies(macroDefinition));
            var visitedMacros = new HashSet<string>();

            while (queue.Count > 0)
            {
                var macro = queue.Dequeue();

                if (visitedMacros.Contains(macro))
                {
                    return true;
                }

                foreach (var dependency in GetMacroDependencies(macros[macro].Definition))
                {
                    queue.Enqueue(dependency);
                }

                visitedMacros.Add(macro);
            }

            return false;
        }
    }

    private List<string> GetMacroDependencies(object? definition)
    {
        if (definition is not List<object?> definitionList || definitionList.Count == 0)
        {
            return new List<string>();
        }

        if (definitionList is ["macro", string])
        {
            return new List<string> { definitionList[1]!.ToString()! };
        }

        return definitionList.SelectMany(GetMacroDependencies).ToList();
    }
}