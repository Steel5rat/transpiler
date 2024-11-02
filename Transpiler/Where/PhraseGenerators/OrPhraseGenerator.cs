using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators;

public class OrPhraseGeneratorFactory : BinaryLogicPhraseGeneratorFactory
{
    public class OrPhraseGenerator : IPhraseGenerator
    {
        private readonly ImmutableList<IPhraseGenerator> _operands;

        public OrPhraseGenerator(ImmutableList<IPhraseGenerator> operands)
        {
            _operands = operands;
        }

        public string GetSql()
        {
            return $"({_operands[0].GetSql()}) OR ({_operands[1].GetSql()})";
        }
    }

    protected override string GetOperatorName() => "or";

    protected override IPhraseGenerator CreateGenerator(ImmutableList<IPhraseGenerator> operands)
    {
        return new OrPhraseGenerator(operands);
    }
}