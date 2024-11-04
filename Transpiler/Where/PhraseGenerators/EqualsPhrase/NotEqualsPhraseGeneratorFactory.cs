using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators.EqualsPhrase;

public class NotEqualsPhraseGeneratorFactory : MatchPhraseGeneratorFactory
{
    public NotEqualsPhraseGeneratorFactory(IEnumerable<IEqualsSyntaxProvider> syntaxProviders) : base(
        syntaxProviders)
    {
    }

    protected override string GetOperationSymbol() => "!=";

    protected override IPhraseGenerator CreatePolyPhraseGenerator(IEqualsSyntaxProvider syntaxProvider,
        ImmutableList<IPhraseGenerator> operands) => new NotEqualsPolyPhraseGenerator(syntaxProvider, operands);

    protected override IPhraseGenerator CreateBinaryPhraseGenerator(IEqualsSyntaxProvider syntaxProvider,
        IPhraseGenerator operand1, IPhraseGenerator operand2) =>
        new NotEqualsBinaryPhraseGenerator(syntaxProvider, operand1, operand2);
}