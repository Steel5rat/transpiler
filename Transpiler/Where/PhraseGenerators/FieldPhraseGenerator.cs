using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators;

public class FieldPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    private readonly ImmutableDictionary<Dialect, IFieldSyntaxProvider> _syntaxProviders;

    public FieldPhraseGeneratorFactory(ImmutableDictionary<Dialect, IFieldSyntaxProvider> syntaxProviders)
    {
        _syntaxProviders = syntaxProviders;
    }

    public class FieldPhraseGenerator : IPhraseGenerator
    {
        private readonly string _fieldName;
        private readonly IFieldSyntaxProvider _syntaxProvider;

        public FieldPhraseGenerator(string fieldName, IFieldSyntaxProvider syntaxProvider)
        {
            _fieldName = fieldName;
            _syntaxProvider = syntaxProvider;
        }

        public string GetSql()
        {
            return _syntaxProvider.Field(_fieldName);
        }
    }

    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> and ["field", _])
        {
            return (true, ImmutableList<object?>.Empty);
        }

        return (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        if (originalOperand is not List<object?> originalOperandList)
        {
            throw new ArgumentException($"{originalOperand} is not a list");
        }

        if (originalOperandList[1] is not int fieldIndex)
        {
            throw new ArgumentException($"{originalOperand} does not contain a field index");
        }

        if (!fields.TryGetValue(fieldIndex, out var fieldName))
        {
            throw new ArgumentException($"field {fieldIndex} not found");
        }

        return new FieldPhraseGenerator(fieldName, _syntaxProviders[dialect]);
    }
}