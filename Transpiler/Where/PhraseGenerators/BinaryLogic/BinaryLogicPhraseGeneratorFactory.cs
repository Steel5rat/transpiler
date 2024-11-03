using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where.PhraseGenerators.BinaryLogic;

public abstract class BinaryLogicPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    protected abstract string GetOperatorName();

    protected abstract IPhraseGenerator CreateGenerator(IPhraseGenerator operand1, IPhraseGenerator operand2);

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

        if (operands.Count > 3)
        {
            throw new InvalidOperationException($"Too many operands: {operands.Count}");
        }

        return CreateGenerator(operands[0], operands[1]);
    }
}