using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators;

public class AndPhraseGeneratorFactory : BinaryLogicPhraseGeneratorFactory
{
    public class AndPhraseGenerator : IPhraseGenerator
    {
        private readonly ImmutableList<IPhraseGenerator> _operands;

        public AndPhraseGenerator(ImmutableList<IPhraseGenerator> operands)
        {
            _operands = operands;
        }

        public string GetSql()
        {
            return $"({_operands[0].GetSql()}) AND ({_operands[1].GetSql()})";
        }
    }

    protected override string GetOperatorName() => "and";

    protected override IPhraseGenerator CreateGenerator(ImmutableList<IPhraseGenerator> operands)
    {
        return new AndPhraseGenerator(operands);
    }
}