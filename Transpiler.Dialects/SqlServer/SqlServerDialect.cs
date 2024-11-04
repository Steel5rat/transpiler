using Transpiler.Engine.Common;
using Transpiler.Engine.Limit;
using Transpiler.Engine.Models;
using Transpiler.Engine.Where;

namespace Transpiler.Dialects.SqlServer;

public class SqlServerDialect : IDialect
{
    private readonly ILimitGenerator _limitGenerator;
    private readonly IWhereGenerator _whereGenerator;

    public bool IsNameMatch(string name) => name == Dialect.SqlServer.ToString().ToLower();

    public SqlServerDialect(ILimitGenerator limitGenerator,
        IWhereGenerator whereGenerator)
    {
        _limitGenerator = limitGenerator;
        _whereGenerator = whereGenerator;
    }

    public string GenerateSelect(Fields fields, Query query)
    {
        return $"""
                SELECT
                {_limitGenerator.GenerateClause(query, Dialect.SqlServer)} *
                FROM data
                {_whereGenerator.GenerateClause(fields, query, Dialect.SqlServer)};
                """;
    }
}