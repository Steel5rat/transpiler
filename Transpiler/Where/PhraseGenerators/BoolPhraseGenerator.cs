using System.Collections.Immutable;
using System.Globalization;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where.PhraseGenerators;

public class BoolPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public class BoolPhraseGenerator : IPhraseGenerator
    {
        private readonly bool _value;

        public BoolPhraseGenerator(bool value)
        {
            _value = value;
        }

        public string GetSql()
        {
            return _value ? "TRUE" : "FALSE";
        }
    }

    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        return operand is bool
            ? (true, ImmutableList<object?>.Empty)
            : (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        return new BoolPhraseGenerator(originalOperand is bool operand ? operand : throw new ArgumentNullException(nameof(originalOperand)));
    }
}