using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Limit;
using Transpiler.MySql;
using Transpiler.MySql.SyntaxProviders;
using Transpiler.Postgres;
using Transpiler.Postgres.SyntaxProviders;
using Transpiler.SqlServer;
using Transpiler.SqlServer.SyntaxProviders;
using Transpiler.Where;
using Transpiler.Where.PhraseGenerators;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler.Tests;

public static class TranspilerFactory
{
    // in production we should use dependency injection for that
    public static Transpiler Create()
    {
        var limitGenerator = new LimitGenerator(new Dictionary<Dialect, ILimitSyntaxProvider>()
        {
            { Dialect.Postgres, new PostgresLimitSyntaxProvider() },
            { Dialect.MySql, new MySqlLimitSyntaxProvider() },
            { Dialect.SqlServer, new SqlServerLimitSyntaxProvider() }
        }.ToImmutableDictionary());

        var whereGenerator = new WhereGenerator(new List<IPhraseGeneratorFactory>()
        {
            new EqualsPhraseGeneratorFactory(new Dictionary<Dialect, IEqualsSyntaxProvider>
            {
                { Dialect.Postgres, new PostgresEqualsSyntaxProvider() },
                { Dialect.MySql, new MySqlEqualsSyntaxProvider() },
                { Dialect.SqlServer, new SqlServerEqualsSyntaxProvider() }
            }.ToImmutableDictionary()),
            new FieldPhraseGeneratorFactory(new Dictionary<Dialect, IFieldSyntaxProvider>
            {
                { Dialect.Postgres, new PostgresFieldSyntaxProvider() },
                { Dialect.MySql, new MySqlFieldSyntaxProvider() },
                { Dialect.SqlServer, new SqlServerFieldSyntaxProvider() }
            }.ToImmutableDictionary()),
            new NullPhraseGeneratorFactory(),
            new NumberPhraseGeneratorFactory(),
            new StringPhraseGeneratorFactory(),
            new OrPhraseGeneratorFactory(),
            new AndPhraseGeneratorFactory(),
            new NotPhraseGeneratorFactory(),
            new EmptyPhraseGeneratorFactory(),
            new ComparisonPhraseGeneratorFactory()
        }.ToImmutableList());

        return new Transpiler(new List<IDialect>
        {
            new PostgresDialect(limitGenerator, whereGenerator),
            new MySqlDialect(limitGenerator, whereGenerator),
            new SqlServerDialect(limitGenerator, whereGenerator)
        }.ToImmutableList());
    }
}