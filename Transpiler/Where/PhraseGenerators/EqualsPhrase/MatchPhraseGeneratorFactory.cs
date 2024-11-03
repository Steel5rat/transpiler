using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.PhraseGenerators.Field;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Where.PhraseGenerators.EqualsPhrase;

public abstract class MatchPhraseGeneratorFactory : IPhraseGeneratorFactory
{
    private readonly ImmutableDictionary<Dialect, IEqualsSyntaxProvider> _syntaxProviders;

    protected MatchPhraseGeneratorFactory(ImmutableDictionary<Dialect, IEqualsSyntaxProvider> syntaxProviders)
    {
        _syntaxProviders = syntaxProviders;
    }

    public (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand)
    {
        if (operand is List<object?> { Count: >= 3 } operandsAsList && operandsAsList[0]?.ToString() == GetOperationSymbol())
        {
            return (true, operandsAsList.Skip(1).ToImmutableList());
        }

        return (false, ImmutableList<object?>.Empty);
    }

    public IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands,
        Fields fields, Dialect dialect)
    {
        var invalidOperands = operands.Where(o => !o.IsPrimitive() && o is not FieldPhraseGenerator).ToImmutableList();
        if (invalidOperands.Any())
        {
            throw new InvalidOperationException(
                $"Invalid operands types: {string.Join(", ", invalidOperands.Select(o => o.GetSql()))}");
        }

        return operands.Count > 2
            ? CreatePolyPhraseGenerator(_syntaxProviders[dialect], operands)
            : CreateBinaryPhraseGenerator(_syntaxProviders[dialect], operands[0], operands[1]);
    }

    protected abstract string GetOperationSymbol();

    protected abstract IPhraseGenerator CreatePolyPhraseGenerator(IEqualsSyntaxProvider syntaxProvider, ImmutableList<IPhraseGenerator> operands);
    protected abstract IPhraseGenerator CreateBinaryPhraseGenerator(IEqualsSyntaxProvider syntaxProvider, IPhraseGenerator operand1, IPhraseGenerator operand2);
}