using System.Collections.Immutable;
using Transpiler.Engine.Common;
using Transpiler.Engine.Models;

namespace Transpiler.Engine.Where.PhraseGenerators.NotPhrase;

public class NotPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> operandsAsList and ["not", _])
        {
            return (true, operandsAsList.Skip(1).ToImmutableList());
        }

        return (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? _, ImmutableList<IPhraseGenerator> operands, Fields fields,
        Dialect dialect)
    {
        var operand = operands.Count switch
        {
            1 => operands.First(),
            _ => throw new ArgumentException(
                $"{nameof(operands)} expected to have single element. It contains {operands.Count}")
        };
        if (operand is NotPhraseGenerator notPhraseGenerator) // optimization, if we have nested not, we can skip both
        {
            return notPhraseGenerator.Operand;
        }

        return new NotPhraseGenerator(operand);
    }
}