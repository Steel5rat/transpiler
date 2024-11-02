using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators;

public class EqualsPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public enum OperationType
    {
        Equals,
        NotEquals
    }

    private readonly ImmutableDictionary<Dialect, IEqualsSyntaxProvider> _syntaxProviders;

    public EqualsPhraseGeneratorFactory(ImmutableDictionary<Dialect, IEqualsSyntaxProvider> syntaxProviders)
    {
        _syntaxProviders = syntaxProviders;
    }

    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> operandsAsList and (["=", _, _, ..] or ["!=", _, _, ..]))
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
             throw new InvalidOperationException($"{nameof(originalOperand)} should be a list")).First().ToString() ==
            "="
                ? OperationType.Equals
                : OperationType.NotEquals;
        return new EqualsPhraseGenerator(operationType, operands, _syntaxProviders[dialect]);
    }

    public class EqualsPhraseGenerator : IPhraseGenerator
    {
        private readonly OperationType _operationType;
        private readonly ImmutableList<IPhraseGenerator> _operands;
        private readonly IEqualsSyntaxProvider _syntaxProvider;

        public EqualsPhraseGenerator(OperationType operationType, ImmutableList<IPhraseGenerator> operands,
            IEqualsSyntaxProvider syntaxProvider)
        {
            _operationType = operationType;
            _operands = operands;
            _syntaxProvider = syntaxProvider;
        }

        public string GetSql()
        {
            if (_operands.Count > 2)
            {
                return _operationType == OperationType.Equals
                    ? _syntaxProvider.AreEqual(_operands[0].GetSql(), _operands.Skip(1).Select(o => o.GetSql()))
                    : _syntaxProvider.AreNotEqual(_operands[0].GetSql(), _operands.Skip(1).Select(o => o.GetSql()));
            }

            if (_operands[0].IsNull() && _operands[1].IsNull())
            {
                return _operationType == OperationType.Equals ? "TRUE" : "FALSE";
            }

            var (operand1, operand2) =
                _operands[0].IsNull() ? (_operands[1], _operands[1]) : (_operands[0], _operands[1]);

            if (_operationType == OperationType.Equals)
            {
                return operand2.IsNull()
                    ? _syntaxProvider.IsNull(operand1.GetSql())
                    : _syntaxProvider.AreEqual(operand1.GetSql(), operand2.GetSql());
            }

            return operand2.IsNull()
                ? _syntaxProvider.IsNotNull(operand1.GetSql())
                : _syntaxProvider.AreNotEqual(operand1.GetSql(), operand2.GetSql());
        }
    }
}