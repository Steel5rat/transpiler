using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where.PhraseGenerators.Bool;

public class BoolPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        return operand is bool
            ? (true, ImmutableList<object?>.Empty)
            : (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        return new BoolPhraseGenerator(originalOperand is bool operand ? operand : throw new ArgumentNullException(nameof(originalOperand)));
    }
}