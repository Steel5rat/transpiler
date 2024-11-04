using System.Collections.Immutable;
using Transpiler.Engine.Common;
using Transpiler.Engine.Models;

namespace Transpiler.Engine.Limit;

public class LimitGenerator : ILimitGenerator
{
    private const string LimitKey = "limit";
    
    private readonly ImmutableDictionary<Dialect, ILimitSyntaxProvider> _syntaxProviders;

    public LimitGenerator(IEnumerable<ILimitSyntaxProvider> syntaxProviders)
    {
        _syntaxProviders = syntaxProviders.ToDictionary(p => p.Dialect, p => p)
            .ToImmutableDictionary();
    }

    public string GenerateClause(Query query, Dialect dialect)
    {
        if (!query.TryGetValue(LimitKey, out var limitObject))
        {
            return "";
        }

        var limitNumber = limitObject as long? ?? limitObject as int?;
        if (limitNumber is not > 0)
        {
            throw new ArgumentException("Limit number must be a positive integer.");
        }

        return _syntaxProviders[dialect].GetLimit(limitNumber.Value);
    }
}