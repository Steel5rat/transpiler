using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Limit;
using Transpiler.MySql;
using Transpiler.MySql.SyntaxProviders;
using Transpiler.Postgres;
using Transpiler.Postgres.SyntaxProviders;
using Transpiler.Redshift;
using Transpiler.SqlServer;
using Transpiler.SqlServer.SyntaxProviders;
using Transpiler.Where;
using Transpiler.Where.PhraseGenerators;
using Transpiler.Where.PhraseGenerators.BinaryLogic;
using Transpiler.Where.PhraseGenerators.Bool;
using Transpiler.Where.PhraseGenerators.Comparison;
using Transpiler.Where.PhraseGenerators.Empty;
using Transpiler.Where.PhraseGenerators.EqualsPhrase;
using Transpiler.Where.PhraseGenerators.Field;
using Transpiler.Where.PhraseGenerators.Like;
using Transpiler.Where.PhraseGenerators.NotPhrase;
using Transpiler.Where.PhraseGenerators.Null;
using Transpiler.Where.PhraseGenerators.Number;
using Transpiler.Where.PhraseGenerators.StringPhrase;
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
            { Dialect.SqlServer, new SqlServerLimitSyntaxProvider() },
            { Dialect.Redshift, new RedshiftLimitSyntaxProvider() }
        }.ToImmutableDictionary());

        var whereGenerator = new WhereGenerator(new List<IPhraseGeneratorFactory>()
        {
            new EqualsPhraseGeneratorFactory(new Dictionary<Dialect, IEqualsSyntaxProvider>
            {
                { Dialect.Postgres, new PostgresEqualsSyntaxProvider() },
                { Dialect.MySql, new MySqlEqualsSyntaxProvider() },
                { Dialect.SqlServer, new SqlServerEqualsSyntaxProvider() },
                { Dialect.Redshift, new RedshiftEqualsSyntaxProvider() }
            }.ToImmutableDictionary()),
            new NotEqualsPhraseGeneratorFactory(new Dictionary<Dialect, IEqualsSyntaxProvider>
            {
                { Dialect.Postgres, new PostgresEqualsSyntaxProvider() },
                { Dialect.MySql, new MySqlEqualsSyntaxProvider() },
                { Dialect.SqlServer, new SqlServerEqualsSyntaxProvider() },
                { Dialect.Redshift, new RedshiftEqualsSyntaxProvider() }
            }.ToImmutableDictionary()),
            new FieldPhraseGeneratorFactory(new Dictionary<Dialect, IFieldSyntaxProvider>
            {
                { Dialect.Postgres, new PostgresFieldSyntaxProvider() },
                { Dialect.MySql, new MySqlFieldSyntaxProvider() },
                { Dialect.SqlServer, new SqlServerFieldSyntaxProvider() },
                { Dialect.Redshift, new RedshiftFieldSyntaxProvider() }
            }.ToImmutableDictionary()),
            new NullPhraseGeneratorFactory(),
            new NumberPhraseGeneratorFactory(),
            new StringPhraseGeneratorFactory(),
            new OrPhraseGeneratorFactory(),
            new AndPhraseGeneratorFactory(),
            new NotPhraseGeneratorFactory(),
            new IsEmptyPhraseGeneratorFactory(),
            new IsNotEmptyPhraseGeneratorFactory(),
            new GreaterPhraseGeneratorFactory(),
            new LowerPhraseGeneratorFactory(),
            new LikePhraseGeneratorFactory(),
            new BoolPhraseGeneratorFactory(),
        }.ToImmutableList());

        return new Transpiler(new List<IDialect>
        {
            new PostgresDialect(limitGenerator, whereGenerator),
            new MySqlDialect(limitGenerator, whereGenerator),
            new SqlServerDialect(limitGenerator, whereGenerator),
            new RedshiftDialect(limitGenerator, whereGenerator)
        }.ToImmutableList());
    }
}