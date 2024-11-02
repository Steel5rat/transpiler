using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators;

public class ComparisonPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public enum OperationType
    {
        Greater,
        Lower
    }

    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> operandsAsList and ([">", _, _] or ["<", _, _]))
        {
            return (true, operandsAsList.Skip(1).ToImmutableList());
        }

        return (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields __, Dialect dialect)
    {
        if (operands.Count != 2)
        {
            throw new ArgumentException($"Expected 2 operands but was {operands.Count}");
        }
        
        var invalidOperands = operands.Where(o => !o.IsNumber() && !o.IsField()).ToImmutableList();
        if (invalidOperands.Any())
        {
            throw new InvalidOperationException(
                $"Invalid operands types: {string.Join(", ", invalidOperands.Select(o => o.GetSql()))}");
        }
            
        var operationType =
            (originalOperand as List<object> ??
             throw new InvalidOperationException($"{nameof(originalOperand)} should be a list")).First().ToString() == ">"
                ? OperationType.Greater
                : OperationType.Lower;
        return new ComparisonPhraseGenerator(operationType, operands.First(), operands.Last());
    }

    public class ComparisonPhraseGenerator : IPhraseGenerator
    {
        private readonly OperationType _operationType;
        private readonly IPhraseGenerator _operand1;
        private readonly IPhraseGenerator _operand2;

        public ComparisonPhraseGenerator(OperationType operationType, IPhraseGenerator operand1, IPhraseGenerator operand2)
        {
            _operationType = operationType;
            _operand1 = operand1;
            _operand2 = operand2;
        }

        public string GetSql()
        {
            return _operationType == OperationType.Greater
                ? $"{_operand1.GetSql()} > {_operand2.GetSql()}"
                : $"{_operand1.GetSql()} < {_operand2.GetSql()}";
        }
    }
}