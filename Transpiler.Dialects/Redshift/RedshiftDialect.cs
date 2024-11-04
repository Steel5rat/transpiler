using Transpiler.Engine.Common;
using Transpiler.Engine.Limit;
using Transpiler.Engine.Models;
using Transpiler.Engine.Where;

namespace Transpiler.Dialects.Redshift;

public class RedshiftDialect : IDialect
{
    private readonly ILimitGenerator _limitGenerator;
    private readonly IWhereGenerator _whereGenerator;
    public bool IsNameMatch(string name) => name == Dialect.Redshift.ToString().ToLower();

    public RedshiftDialect(ILimitGenerator limitGenerator,
        IWhereGenerator whereGenerator)
    {
        _limitGenerator = limitGenerator;
        _whereGenerator = whereGenerator;
    }


    public string GenerateSelect(Fields fields, Query query)
    {
        return $"""
                SELECT *
                FROM data
                {_whereGenerator.GenerateClause(fields, query, Dialect.Redshift)}
                {_limitGenerator.GenerateClause(query, Dialect.Redshift)};
                """;
    }
}