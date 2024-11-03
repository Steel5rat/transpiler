using System.Collections.Immutable;

namespace Transpiler.Where.PhraseGenerators.BinaryLogic;

public class OrPhraseGeneratorFactory : BinaryLogicPhraseGeneratorFactory
{
    protected override string GetOperatorName() => "or";

    protected override IPhraseGenerator CreateGenerator(IPhraseGenerator operand1, IPhraseGenerator operand2)
    {
        return new OrPhraseGenerator(operand1, operand2);
    }
}