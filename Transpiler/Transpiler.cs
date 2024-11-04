using System.Collections.Immutable;
using Transpiler.Engine.Common;
using Transpiler.Engine.Models;

namespace Transpiler;

public class Transpiler : ITranspiler
{
    private readonly ImmutableList<IDialect> _dialects;

    public Transpiler(IEnumerable<IDialect> dialects)
    {
        _dialects = dialects.ToImmutableList();
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