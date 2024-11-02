using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators;

public class EmptyPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public enum OperationType
    {
        IsEmpty,
        IsNotEmpty
    }

    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> operandsAsList and (["is-empty", _] or ["not-empty", _]))
        {
            return (true, operandsAsList.Skip(1).ToImmutableList());
        }

        return (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields __, Dialect dialect)
    {
        var invalidOperands = operands.Where(o => !o.IsPrimitive() && !o.IsField()).ToImmutableList();
        if (invalidOperands.Any())
        {
            throw new InvalidOperationException(
                $"Invalid operands types: {string.Join(", ", invalidOperands.Select(o => o.GetSql()))}");
        }
            
        var operationType =
            (originalOperand as List<object> ??
             throw new InvalidOperationException($"{nameof(originalOperand)} should be a list")).First().ToString() == "is-empty"
                ? OperationType.IsEmpty
                : OperationType.IsNotEmpty;
        return new EqualsPhraseGenerator(operationType, operands.Single());
    }

    public class EqualsPhraseGenerator : IPhraseGenerator
    {
        private readonly OperationType _operationType;
        private readonly IPhraseGenerator _operand;

        public EqualsPhraseGenerator(OperationType operationType, IPhraseGenerator operand)
        {
            _operationType = operationType;
            _operand = operand;
        }

        public string GetSql()
        {
            return _operationType == OperationType.IsEmpty
                ? $"{_operand.GetSql()} IS NULL"
                : $"{_operand.GetSql()} IS NOT NULL";
        }
    }
}