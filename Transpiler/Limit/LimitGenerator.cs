using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Limit;

public class LimitGenerator : ILimitGenerator
{
    private const string LimitKey = "limit";
    
    private readonly ImmutableDictionary<Dialect, ILimitSyntaxProvider> _syntaxProviders;

    public LimitGenerator(ImmutableDictionary<Dialect, ILimitSyntaxProvider> syntaxProviders)
    {
        _syntaxProviders = syntaxProviders;
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