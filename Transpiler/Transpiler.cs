using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler;

public class Transpiler
{
    private readonly ImmutableList<IDialect> _dialects;

    public Transpiler(ImmutableList<IDialect> dialects)
    {
        _dialects = dialects;
    }

    public string GenerateSql(string dialectName, Fields fields, Query query)
    {
        var dialect = GetDialect(dialectName);
        return dialect.GenerateSelect(fields, query);
    }

    private IDialect GetDialect(string dialectName)
    {
        var dialects = _dialects.Where(d => d.IsNameMatch(dialectName)).ToList();
        return dialects.Count switch
        {
            > 1 => throw new ArgumentException($"Dialect {dialectName} is ambiguous"),
            0 => throw new ArgumentException($"Dialect {dialectName} is missing"),
            _ => dialects.Single()
        };
    }
}