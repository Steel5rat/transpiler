namespace Transpiler.Engine.Where.PhraseGenerators.Empty;

public class IsEmptyPhraseGeneratorFactory : EmptyPhraseGeneratorFactory
{
    protected override string GetOperationSymbol() => "is-empty";

    protected override IPhraseGenerator CreatePhraseGenerator(IPhraseGenerator operand) =>
        new IsEmptyPhraseGenerator(operand);
}