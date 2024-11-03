using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.PhraseGenerators.Field;
using Transpiler.Where.PhraseGenerators.StringPhrase;

namespace Transpiler.Where.PhraseGenerators.Like;

public class LikePhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> operandsAsList and ["like", _, _])
        {
            return (true, operandsAsList.Skip(1).ToImmutableList());
        }

        return (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        var (operand1, operand2) = (operands[0], operands[1]);
        if (operand1 is not FieldPhraseGenerator)
        {
            throw new ArgumentException("The first operand must be a field");
        }
        if (operand2 is not StringPhraseGenerator)
        {
            throw new ArgumentException("The second operand must be a string");
        }

        return new LikePhraseGenerator(operand1, operand2);
    }
}