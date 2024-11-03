using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.PhraseGenerators.Field;

namespace Transpiler.Where.PhraseGenerators.Empty;

public abstract class EmptyPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> { Count: 2 } operandsAsList &&
            operandsAsList[0]?.ToString() == GetOperationSymbol())
        {
            return (true, operandsAsList.Skip(1).ToImmutableList());
        }

        return (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        var operand = operands.Single();

        if (!operand.IsPrimitive() && operand is not FieldPhraseGenerator)
        {
            throw new InvalidOperationException($"Invalid operand types: {operand.GetSql()}");
        }

        return CreatePhraseGenerator(operand);
    }

    protected abstract string GetOperationSymbol();

    protected abstract IPhraseGenerator CreatePhraseGenerator(IPhraseGenerator operand);
}