using Transpiler.Common;
using Transpiler.Limit;
using Transpiler.Models;
using Transpiler.Where;

namespace Transpiler.SqlServer;

public class SqlServerDialect : IDialect
{
    private readonly ILimitGenerator _limitGenerator;
    private readonly IWhereGenerator _whereGenerator;

    public bool IsNameMatch(string name) => name == "sqlserver";

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