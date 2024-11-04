using Transpiler.Engine.Common;
using Transpiler.Engine.Models;

namespace Transpiler.Tests.EndToEndTests;

[TestFixture]
public class ExtensionTests
{
    [Test]
    public void RedshiftHappyPath()
    {
        var transpiler = new TranspilerFactory().CreateTranspiler();

        var result = transpiler.GenerateSql(Dialect.Redshift.ToString().ToLower(),
            new Fields { { 1, "id" }, { 2, "name" } },
            new Query()
            {
                {
                    "where",
                    new List<object>
                    {
                        "and",
                        new List<object>
                        {
                            "or",
                            new List<object> { "=", new List<object> { "field", 1 }, 154 },
                            new List<object?> { "=", new List<object> { "field", 1 }, null }
                        },
                        new List<object> { "!=", new List<object> { "field", 2 }, "cam" }
                    }
                },
                { "limit", 10 }
            });

        Assert.That(result, Is.EqualTo($"""
                                        SELECT *
                                        FROM data
                                        WHERE ((<<id>> = 154) OR (<<id>> IS NULL)) AND (<<name>> != 'cam')
                                        MAX LIMIT 10;
                                        """));
    }

    [Test]
    public void LikeClauseHappyPath()
    {
        var transpiler = new TranspilerFactory().CreateTranspiler();

        var result = transpiler.GenerateSql(Dialect.Redshift.ToString().ToLower(),
            new Fields { { 1, "id" }, { 2, "name" } },
            new Query()
            {
                {
                    "where",
                    new List<object> { "like", new List<object> { "field", 2 }, "%cam%" }
                },
                { "limit", 10 }
            });

        Assert.That(result, Is.EqualTo($"""
                                        SELECT *
                                        FROM data
                                        WHERE <<name>> LIKE '%cam%'
                                        MAX LIMIT 10;
                                        """));
    }

    [Test]
    public void BoolLiteralHappyPath()
    {
        var transpiler = new TranspilerFactory().CreateTranspiler();

        var result = transpiler.GenerateSql(Dialect.Postgres.ToString().ToLower(),
            new Fields { { 1, "id" }, { 2, "name" } },
            new Query()
            {
                {
                    "where",
                    new List<object> { "=", new List<object> { "field", 2 }, true }
                },
                { "limit", 10 }
            });

        Assert.That(result, Is.EqualTo($"""
                                        SELECT *
                                        FROM data
                                        WHERE "name" = TRUE
                                        LIMIT 10;
                                        """));
    }
}