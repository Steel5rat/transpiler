using System.Collections.Immutable;
using System.Globalization;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where.PhraseGenerators;

public class NumberPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public class NumberPhraseGenerator : IPhraseGenerator
    {
        private readonly decimal _value;

        public NumberPhraseGenerator(decimal value)
        {
            _value = value;
        }
        public string GetSql()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        return operand is double or float or decimal or long or int
            ? (true, ImmutableList<object?>.Empty)
            : (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        return new NumberPhraseGenerator(Convert.ToDecimal(originalOperand));
    }
}