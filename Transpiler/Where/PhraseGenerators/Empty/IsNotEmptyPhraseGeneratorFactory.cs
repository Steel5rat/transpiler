namespace Transpiler.Where.PhraseGenerators.Empty;

public class IsNotEmptyPhraseGeneratorFactory : EmptyPhraseGeneratorFactory
{
    protected override string GetOperationSymbol() => "not-empty";

    protected override IPhraseGenerator CreatePhraseGenerator(IPhraseGenerator operand) =>
        new IsNotEmptyPhraseGenerator(operand);
}