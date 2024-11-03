using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where.PhraseGenerators.StringPhrase;

public class StringPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        return operand is string
            ? (true, ImmutableList<object?>.Empty)
            : (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        return new StringPhraseGenerator(originalOperand?.ToString() ??
                                         throw new ArgumentNullException(nameof(originalOperand)));
    }
}