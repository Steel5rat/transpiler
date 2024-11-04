using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Transpiler.Dialects.MySql;
using Transpiler.Dialects.MySql.SyntaxProviders;
using Transpiler.Dialects.Postgres;
using Transpiler.Dialects.Postgres.SyntaxProviders;
using Transpiler.Dialects.Redshift;
using Transpiler.Dialects.Redshift.SyntaxProviders;
using Transpiler.Dialects.SqlServer;
using Transpiler.Dialects.SqlServer.SyntaxProviders;
using Transpiler.Engine.Common;
using Transpiler.Engine.Limit;
using Transpiler.Engine.Where;
using Transpiler.Engine.Where.PhraseGenerators;
using Transpiler.Engine.Where.PhraseGenerators.BinaryLogic;
using Transpiler.Engine.Where.PhraseGenerators.Bool;
using Transpiler.Engine.Where.PhraseGenerators.Comparison;
using Transpiler.Engine.Where.PhraseGenerators.Empty;
using Transpiler.Engine.Where.PhraseGenerators.EqualsPhrase;
using Transpiler.Engine.Where.PhraseGenerators.Field;
using Transpiler.Engine.Where.PhraseGenerators.Like;
using Transpiler.Engine.Where.PhraseGenerators.Macro;
using Transpiler.Engine.Where.PhraseGenerators.NotPhrase;
using Transpiler.Engine.Where.PhraseGenerators.Null;
using Transpiler.Engine.Where.PhraseGenerators.Number;
using Transpiler.Engine.Where.PhraseGenerators.StringPhrase;
using Transpiler.Engine.Where.SyntaxProviders;

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