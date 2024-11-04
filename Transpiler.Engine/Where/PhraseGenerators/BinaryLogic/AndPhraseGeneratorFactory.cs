namespace Transpiler.Engine.Where.PhraseGenerators.BinaryLogic;

public class AndPhraseGeneratorFactory : BinaryLogicPhraseGeneratorFactory
{
    protected override string GetOperatorName() => "and";

    protected override IPhraseGenerator CreateGenerator(IPhraseGenerator operand1, IPhraseGenerator operand2)
    {
        return new AndPhraseGenerator(operand1, operand2);
    }
}