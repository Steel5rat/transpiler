using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.PhraseGenerators.Field;
using Transpiler.Where.PhraseGenerators.Number;

namespace Transpiler.Where.PhraseGenerators.Comparison;

public abstract class ComparisonPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> { Count: 3 } operandsAsList &&
            operandsAsList[0]?.ToString() == GetOperationSymbol())
        {
            return (true, operandsAsList.Skip(1).ToImmutableList());
        }

        return (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        if (operands.Count != 2)
        {
            throw new ArgumentException($"Expected 2 operands but was {operands.Count}");
        }

        var invalidOperands = operands.Where(o => o is not NumberPhraseGenerator && o is not FieldPhraseGenerator).ToImmutableList();
        if (invalidOperands.Any())
        {
            throw new InvalidOperationException(
                $"Invalid operands types: {string.Join(", ", invalidOperands.Select(o => o.GetSql()))}");
        }

        return CreatePhraseGenerator(operands.First(), operands.Last());
    }

    protected abstract string GetOperationSymbol();

    protected abstract IPhraseGenerator CreatePhraseGenerator(IPhraseGenerator operand1, IPhraseGenerator operand2);
}