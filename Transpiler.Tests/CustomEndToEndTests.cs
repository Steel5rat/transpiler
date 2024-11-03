using System.Collections;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Tests;

[TestFixture]
public class CustomEndToEndTests
{
    [Test, TestCaseSource(nameof(EqualsTestCases))]
    public string EqualsTests(Dialect dialect, List<object?> whereClause)
    {
        var transpiler = TranspilerFactory.Create();

        var result = transpiler.GenerateSql(dialect.ToString().ToLower(), new Fields { { 1, "id" }, { 2, "name" } },
            new Query()
            {
                { "where", whereClause }
            });
        return result;
    }

    [Test, TestCaseSource(nameof(LimitTestCases))]
    public string LimitTests(Dialect dialect)
    {
        var transpiler = TranspilerFactory.Create();

        return transpiler.GenerateSql(dialect.ToString().ToLower(), new Fields { { 1, "id" }, { 2, "name" } },
            new Query()
            {
                { "where", new List<object> { "=", new List<object> { "field", 2 }, "cam", 33 } },
                { "limit", 10 }
            });
    }

    [Test]
    public void LogicOperatorsHappyPath()
    {
        var transpiler = TranspilerFactory.Create();

        var result = transpiler.GenerateSql(Dialect.Postgres.ToString().ToLower(),
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
                        new List<object> { "=", new List<object> { "field", 2 }, "cam", "mac" }
                    }
                },
                { "limit", 10 }
            });

        Assert.That(result, Is.EqualTo($"""
                                        SELECT *
                                        FROM data
                                        WHERE (("id" = 154) OR ("id" IS NULL)) AND ("name" IN ('cam', 'mac'))
                                        LIMIT 10;
                                        """));
    }

    [Test]
    public void LogicOperatorOptimizationWithSingleOperand()
    {
        var transpiler = TranspilerFactory.Create();

        var result = transpiler.GenerateSql(Dialect.Postgres.ToString().ToLower(),
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
                            new List<object> { "=", new List<object> { "field", 1 }, 154 }
                        }
                    }
                }
            });

        Assert.That(result, Is.EqualTo($"""
                                        SELECT *
                                        FROM data
                                        WHERE "id" = 154
                                        ;
                                        """));
    }

    [Test]
    public void NotOperatorHappyPath()
    {
        var transpiler = TranspilerFactory.Create();

        var result = transpiler.GenerateSql(Dialect.Postgres.ToString().ToLower(),
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
                            "not",
                            new List<object> { "=", new List<object> { "field", 1 }, 154 }
                        },
                        new List<object> { "=", new List<object> { "field", 2 }, "cam", "mac" }
                    }
                }
            });

        Assert.That(result, Is.EqualTo($"""
                                        SELECT *
                                        FROM data
                                        WHERE (NOT ("id" = 154)) AND ("name" IN ('cam', 'mac'))
                                        ;
                                        """));
    }

    [Test]
    public void NotOperatorOptimization()
    {
        var transpiler = TranspilerFactory.Create();

        var result = transpiler.GenerateSql(Dialect.Postgres.ToString().ToLower(),
            new Fields { { 1, "id" }, { 2, "name" } },
            new Query()
            {
                {
                    "where",
                    new List<object>
                    {
                        "not",
                        new List<object>
                        {
                            "not",
                            new List<object>
                            {
                                "not",
                                new List<object> { "!=", new List<object> { "field", 1 }, 154 }
                            }
                        }
                    }
                }
            });

        Assert.That(result, Is.EqualTo($"""
                                        SELECT *
                                        FROM data
                                        WHERE NOT ("id" <> 154)
                                        ;
                                        """));
    }
    
    [Test]
    public void EmptyHappyPath()
    {
        var transpiler = TranspilerFactory.Create();

        var result = transpiler.GenerateSql(Dialect.Postgres.ToString().ToLower(),
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
                            new List<object?> { "is-empty", new List<object> { "field", 1 } }
                        },
                        new List<object> { "not-empty", new List<object> { "field", 2 } }
                    }
                },
                { "limit", 10 }
            });

        Assert.That(result, Is.EqualTo($"""
                                        SELECT *
                                        FROM data
                                        WHERE (("id" = 154) OR ("id" IS NULL)) AND ("name" IS NOT NULL)
                                        LIMIT 10;
                                        """));
    }
    
    [Test]
    public void ComparisonHappyPath()
    {
        var transpiler = TranspilerFactory.Create();

        var result = transpiler.GenerateSql(Dialect.Postgres.ToString().ToLower(),
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
                            new List<object> { ">", new List<object> { "field", 1 }, 154 },
                            new List<object?> { "is-empty", new List<object> { "field", 1 } }
                        },
                        new List<object> { "<", new List<object> { "field", 2 }, -10 }
                    }
                }
            });

        Assert.That(result, Is.EqualTo($"""
                                        SELECT *
                                        FROM data
                                        WHERE (("id" > 154) OR ("id" IS NULL)) AND ("name" < -10)
                                        ;
                                        """));
    }


    private static IEnumerable EqualsTestCases()
    {
        yield return new TestCaseData(Dialect.Postgres,
                new List<object> { "=", new List<object> { "field", 2 }, "cam" })
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE "name" = 'cam'
                      ;
                      """);
        yield return new TestCaseData(Dialect.MySql,
                new List<object> { "=", new List<object> { "field", 2 }, "cam" })
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE `name` = 'cam'
                      ;
                      """);
        yield return new TestCaseData(Dialect.SqlServer,
                new List<object> { "=", new List<object> { "field", 2 }, "cam" })
            .Returns($"""
                      SELECT
                       *
                      FROM data
                      WHERE "name" = 'cam';
                      """);
        yield return new TestCaseData(Dialect.Postgres,
                new List<object> { "=", new List<object> { "field", 2 }, "cam", 33 })
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE "name" IN ('cam', 33)
                      ;
                      """);
        yield return new TestCaseData(Dialect.MySql,
                new List<object> { "=", new List<object> { "field", 2 }, "cam", 33 })
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE `name` IN ('cam', 33)
                      ;
                      """);
        yield return new TestCaseData(Dialect.SqlServer,
                new List<object> { "=", new List<object> { "field", 2 }, "cam", 33 })
            .Returns($"""
                      SELECT
                       *
                      FROM data
                      WHERE "name" IN ('cam', 33);
                      """);
        yield return new TestCaseData(Dialect.Postgres,
                new List<object?> { "=", new List<object> { "field", 2 }, null })
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE "name" IS NULL
                      ;
                      """);
        yield return new TestCaseData(Dialect.MySql,
                new List<object?> { "=", new List<object> { "field", 2 }, null })
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE `name` IS NULL
                      ;
                      """);
        yield return new TestCaseData(Dialect.SqlServer,
                new List<object?> { "=", new List<object> { "field", 2 }, null })
            .Returns($"""
                      SELECT
                       *
                      FROM data
                      WHERE "name" IS NULL;
                      """);
    }

    private static IEnumerable LimitTestCases()
    {
        yield return new TestCaseData(Dialect.Postgres).Returns($"""
                                                                 SELECT *
                                                                 FROM data
                                                                 WHERE "name" IN ('cam', 33)
                                                                 LIMIT 10;
                                                                 """);
        yield return new TestCaseData(Dialect.MySql).Returns($"""
                                                              SELECT *
                                                              FROM data
                                                              WHERE `name` IN ('cam', 33)
                                                              LIMIT 10;
                                                              """);
        yield return new TestCaseData(Dialect.SqlServer).Returns($"""
                                                                  SELECT
                                                                  TOP 10 *
                                                                  FROM data
                                                                  WHERE "name" IN ('cam', 33);
                                                                  """);
    }
}