using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where.PhraseGenerators;


public class NullPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public class NullPhraseGenerator : IPhraseGenerator
    {
        public string GetSql()
        {
            return "NULL";
        }
    }

    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        return (operand is null, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands, Fields fields, Dialect dialect)
    {
        return new NullPhraseGenerator();
    }
}
