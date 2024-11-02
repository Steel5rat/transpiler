using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators;

public class LikePhraseGeneratorFactory : IPhraseGeneratorFactory
{
    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> operandsAsList and ["like", _, _])
        {
            return (true, operandsAsList.Skip(1).ToImmutableList());
        }

        return (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields __, Dialect dialect)
    {
        var (operand1, operand2) = (operands[0], operands[1]);
        if (!operand1.IsField())
        {
            throw new ArgumentException("The first operand must be a field");
        }
        if (!operand2.IsString())
        {
            throw new ArgumentException("The second operand must be a string");
        }

        return new LikePhraseGenerator(operand1, operand2);
    }

    public class LikePhraseGenerator : IPhraseGenerator
    {
        private readonly IPhraseGenerator _operand1;
        private readonly IPhraseGenerator _operand2;

        public LikePhraseGenerator(IPhraseGenerator operand1, IPhraseGenerator operand2)
        {
            _operand1 = operand1;
            _operand2 = operand2;
        }

        public string GetSql()
        {
            return $"{_operand1.GetSql()} LIKE {_operand2.GetSql()}";
        }
    }
}