using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where.PhraseGenerators.Macro;

public class MacroPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    private readonly IImmutableDictionary<string, Macro> _macros;

    public MacroPhraseGeneratorFactory(IImmutableDictionary<string, Macro> macros)
    {
        _macros = macros;
    }

    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is not (List<object> and ["macro", string] operands))
        {
            return (false, ImmutableList<object?>.Empty);
        }

        var macroName = operands[1] as string;
        if (!_macros.TryGetValue(macroName!, out var macro))
        {
            throw new InvalidOperationException($"Unknown macro: {macroName}");
        }

        return (true, new List<object?> { macro.Definition }.ToImmutableList());
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        return operands.Single();
    }
}