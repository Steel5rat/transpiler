namespace Transpiler.Where.PhraseGenerators.Comparison;

public class GreaterPhraseGeneratorFactory : ComparisonPhraseGeneratorFactory
{
    protected override string GetOperationSymbol() => ">";

    protected override IPhraseGenerator CreatePhraseGenerator(IPhraseGenerator operand1, IPhraseGenerator operand2) =>
        new GreaterPhraseGenerator(operand1, operand2);
}