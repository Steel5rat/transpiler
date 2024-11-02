using Transpiler.Common;
using Transpiler.Limit;
using Transpiler.Models;
using Transpiler.Where;

namespace Transpiler.Postgres;

public class PostgresDialect : IDialect
{
    private readonly ILimitGenerator _limitGenerator;
    private readonly IWhereGenerator _whereGenerator;

    public bool IsNameMatch(string name) => name == "postgres";

    public PostgresDialect(ILimitGenerator limitGenerator,
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
                {_whereGenerator.GenerateClause(fields, query, Dialect.Postgres)}
                {_limitGenerator.GenerateClause(query, Dialect.Postgres)};
                """;
    }
}