using Transpiler.Engine.Common;
using Transpiler.Engine.Models;

namespace Transpiler.Tests.EndToEndTests;

[TestFixture]
public class TestsFromTask
{
    [Test, TestCaseSource(nameof(ExamplesTestCases))]
    public string ExamplesTests(Dialect dialect, List<object?>? whereClause, int? limitClause)
    {
        var transpiler = new TranspilerFactory().CreateTranspiler();
        var query = new Query();
        if (whereClause is not null)
        {
            query["where"] = whereClause;
        }

        if (limitClause is not null)
        {
            query["limit"] = limitClause;
        }

        var result = transpiler.GenerateSql(dialect.ToString().ToLower(),
            new Fields { { 1, "id" }, { 2, "name" } }, query);
        return result;
    }
    
    [Test, TestCaseSource(nameof(RequirementsTestCases))]
    public string RequirementsTests(Dialect dialect, List<object?>? whereClause, int? limitClause)
    {
        var transpiler = new TranspilerFactory().CreateTranspiler();
        var query = new Query();
        if (whereClause is not null)
        {
            query["where"] = whereClause;
        }

        if (limitClause is not null)
        {
            query["limit"] = limitClause;
        }

        var result = transpiler.GenerateSql(dialect.ToString().ToLower(),
            new Fields { { 1, "id" }, { 2, "name" }, {3, "date_joined"}, {4, "age"} }, query);
        return result;
    }

    private static IEnumerable<TestCaseData> ExamplesTestCases()
    {
        yield return new TestCaseData(Dialect.Postgres,
                new List<object> { "=", new List<object> { "field", 2 }, "cam" }, null)
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE "name" = 'cam'
                      ;
                      """);
        yield return new TestCaseData(Dialect.MySql,
                new List<object> { "=", new List<object> { "field", 2 }, "cam" }, 10)
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE `name` = 'cam'
                      LIMIT 10;
                      """);
        yield return new TestCaseData(Dialect.Postgres, null, 20)
            .Returns($"""
                      SELECT *
                      FROM data
                      
                      LIMIT 20;
                      """);
        yield return new TestCaseData(Dialect.SqlServer, null, 20)
            .Returns($"""
                      SELECT
                      TOP 20 *
                      FROM data
                      ;
                      """);
    }

    private static IEnumerable<TestCaseData> RequirementsTestCases()
    {
        yield return new TestCaseData(Dialect.Postgres,
                new List<object> { "=", new List<object> { "field", 3 }, null }, null)
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE "date_joined" IS NULL
                      ;
                      """);
        
        yield return new TestCaseData(Dialect.Postgres,
                new List<object> { ">", new List<object> { "field", 4 }, 35 }, null)
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE "age" > 35
                      ;
                      """);
        
        yield return new TestCaseData(Dialect.Postgres,
                new List<object> { "and", 
                    new List<object> { "<", new List<object>() {"field", 1}, 5 }, 
                    new List<object> { "=", new List<object>() {"field", 2}, "joe"}}, 
                null)
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE ("id" < 5) AND ("name" = 'joe')
                      ;
                      """);
        
        yield return new TestCaseData(Dialect.Postgres,
                new List<object> { "or", 
                    new List<object> { "!=", new List<object>() {"field", 3}, "2015-11-01" }, 
                    new List<object> { "=", new List<object>() {"field", 1}, 456}}, 
                null)
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE ("date_joined" <> '2015-11-01') OR ("id" = 456)
                      ;
                      """);
        
        yield return new TestCaseData(Dialect.Postgres,
                new List<object> { "and", 
                    new List<object?> { "!=", new List<object>() {"field", 3}, null }, 
                    new List<object> { "or", 
                        new List<object>{">", new List<object>() {"field", 4}, 25}, 
                        new List<object>{"=", new List<object>() {"field", 2}, "Jerry"}}}, 
                null)
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE ("date_joined" IS NOT NULL) AND (("age" > 25) OR ("name" = 'Jerry'))
                      ;
                      """);
        
        yield return new TestCaseData(Dialect.Postgres,
                new List<object?> { "=", new List<object> { "field", 4 }, 25, 26, 27 }, 
                null)
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE "age" IN (25, 26, 27)
                      ;
                      """);
        
        yield return new TestCaseData(Dialect.Postgres,
                new List<object> { "=", new List<object> { "field", 2 }, "cam" }, null)
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE "name" = 'cam'
                      ;
                      """);
        
        yield return new TestCaseData(Dialect.MySql,
                new List<object> { "=", new List<object> { "field", 2 }, "cam" }, 10)
            .Returns($"""
                      SELECT *
                      FROM data
                      WHERE `name` = 'cam'
                      LIMIT 10;
                      """);
        
        yield return new TestCaseData(Dialect.Postgres, null, 20)
            .Returns($"""
                      SELECT *
                      FROM data
                      
                      LIMIT 20;
                      """);
        
        yield return new TestCaseData(Dialect.SqlServer, null, 20)
            .Returns($"""
                      SELECT
                      TOP 20 *
                      FROM data
                      ;
                      """);
    }

}