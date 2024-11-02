using Transpiler.Common;
using Transpiler.Limit;
using Transpiler.Models;
using Transpiler.Where;

namespace Transpiler.MySql;

public class MySqlDialect : IDialect
{
    private readonly ILimitGenerator _limitGenerator;
    private readonly IWhereGenerator _whereGenerator;
    public bool IsNameMatch(string name) => name == "mysql";
    
    public MySqlDialect(ILimitGenerator limitGenerator,
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
                {_whereGenerator.GenerateClause(fields, query, Dialect.MySql)}
                {_limitGenerator.GenerateClause(query, Dialect.MySql)};
                """;
    }
}