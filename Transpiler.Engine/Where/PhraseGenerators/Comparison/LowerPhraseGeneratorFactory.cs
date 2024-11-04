namespace Transpiler.Engine.Where.PhraseGenerators.Comparison;

public class LowerPhraseGeneratorFactory : ComparisonPhraseGeneratorFactory
{
    protected override string GetOperationSymbol() => "<";

    protected override IPhraseGenerator CreatePhraseGenerator(IPhraseGenerator operand1, IPhraseGenerator operand2) =>
        new LowerPhraseGenerator(operand1, operand2);
}