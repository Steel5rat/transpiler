using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators;

public abstract class BinaryLogicPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    protected abstract string GetOperatorName();

    protected abstract IPhraseGenerator CreateGenerator(ImmutableList<IPhraseGenerator> operands);

    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> operandsAsList)
        {
            if (operandsAsList.Count is <= 3 and >= 2 && operandsAsList[0]?.ToString() == GetOperatorName())
            {
                return (true, operandsAsList.Skip(1).ToImmutableList());
            }
        }

        return (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        if (originalOperand is List<object?> { Count: 2 }) // looks like ['and', <some expression>], we can skip current operator at all
        {
            return operands.First();
        }
        return CreateGenerator(operands);
    }
}