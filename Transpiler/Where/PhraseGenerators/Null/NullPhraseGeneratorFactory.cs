using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where.PhraseGenerators.Null;


public class NullPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        return (operand is null, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands, Fields fields, Dialect dialect)
    {
        return new NullPhraseGenerator();
    }
}
