using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
using Transpiler.Where.PhraseGenerators.Macro;
using Transpiler.Where.PhraseGenerators.NotPhrase;
using Transpiler.Where.PhraseGenerators.Null;
using Transpiler.Where.PhraseGenerators.Number;
using Transpiler.Where.PhraseGenerators.StringPhrase;
using Transpiler.Where.SyntaxProviders;

namespace Transpiler;

public static class DependenciesContainer
{
    public static IHost GetContainer(Dictionary<string, List<object>> macros)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<ILimitGenerator, LimitGenerator>();
                services.AddSingleton<ILimitSyntaxProvider, PostgresLimitSyntaxProvider>();
                services.AddSingleton<ILimitSyntaxProvider, MySqlLimitSyntaxProvider>();
                services.AddSingleton<ILimitSyntaxProvider, SqlServerLimitSyntaxProvider>();
                services.AddSingleton<ILimitSyntaxProvider, RedshiftLimitSyntaxProvider>();
                
                services.AddSingleton<IWhereGenerator, WhereGenerator>();
                services.AddSingleton<IEqualsSyntaxProvider, PostgresEqualsSyntaxProvider>();
                services.AddSingleton<IEqualsSyntaxProvider, MySqlEqualsSyntaxProvider>();
                services.AddSingleton<IEqualsSyntaxProvider, SqlServerEqualsSyntaxProvider>();
                services.AddSingleton<IEqualsSyntaxProvider, RedshiftEqualsSyntaxProvider>();
                services.AddSingleton<IFieldSyntaxProvider, PostgresFieldSyntaxProvider>();
                services.AddSingleton<IFieldSyntaxProvider, MySqlFieldSyntaxProvider>();
                services.AddSingleton<IFieldSyntaxProvider, SqlServerFieldSyntaxProvider>();
                services.AddSingleton<IFieldSyntaxProvider, RedshiftFieldSyntaxProvider>();
                services.AddSingleton<IPhraseGeneratorFactory, EqualsPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, NotEqualsPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, FieldPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, NullPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, NumberPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, StringPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, OrPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, AndPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, NotPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, IsEmptyPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, IsNotEmptyPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, GreaterPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, LowerPhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, LikePhraseGeneratorFactory>();
                services.AddSingleton<IPhraseGeneratorFactory, BoolPhraseGeneratorFactory>();
                
                services.AddSingleton<IMacroFactory, MacroFactory>();
                services.AddSingleton<IPhraseGeneratorFactory>(sp => 
                    new MacroPhraseGeneratorFactory(sp.GetRequiredService<IMacroFactory>().CreateMacros(macros)));
                
                services.AddSingleton<IDialect, PostgresDialect>();
                services.AddSingleton<IDialect, RedshiftDialect>();
                services.AddSingleton<IDialect, SqlServerDialect>();
                services.AddSingleton<IDialect, MySqlDialect>();
                
                services.AddSingleton<ITranspiler, Transpiler>();
            })
            .Build();

         return host;
    }
}