using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;
using Transpiler.Where.PhraseGenerators;

namespace Transpiler.Where;

public class WhereGenerator : IWhereGenerator
{
    private readonly ImmutableList<IPhraseGeneratorFactory> _phraseGeneratorFactories;
    private const string WhereKey = "where";

    public WhereGenerator(ImmutableList<IPhraseGeneratorFactory> phraseGeneratorFactories)
    {
        _phraseGeneratorFactories = phraseGeneratorFactories;
    }

    public string GenerateClause(Fields fields, Query query, Dialect dialect)
    {
        if (!query.TryGetValue(WhereKey, out var whereObject))
        {
            return "";
        }

        if (whereObject is not List<object?> whereClauses)
        {
            throw new ArgumentException($"Invalid query parameter: {whereObject}");
        }

        var whereSql = CreatePhraseGenerator(whereClauses).GetSql();
        return string.IsNullOrEmpty(whereSql) ? "" : $"WHERE {whereSql}";

        IPhraseGenerator CreatePhraseGenerator(object? clause)
        {
            IPhraseGenerator? result = null;
            foreach (var phraseGeneratorFactory in _phraseGeneratorFactories)
            {
                var (isMatch, operandsToBeConverted) = phraseGeneratorFactory.IsMatch(clause);
                if (isMatch)
                {
                    if (result is not null)
                    {
                        throw new ArgumentException($"Ambiguous phrase: {clause}");
                    }

                    var convertedOperands = operandsToBeConverted.Select(CreatePhraseGenerator).ToImmutableList();
                    result = phraseGeneratorFactory.CreateGenerator(clause, convertedOperands, fields, dialect);
                }
            }

            if (result is null)
            {
                throw new ArgumentException($"Invalid clause {clause}, valid generator factory not found");
            }

            return result;
        }
    }
}