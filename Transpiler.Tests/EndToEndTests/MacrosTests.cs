using Transpiler.Engine.Models;

namespace Transpiler.Tests.EndToEndTests;

[TestFixture]
public class MacrosTests
{
    [Test]
    public void MacrosHappyPath()
    {
        var transpiler = new TranspilerFactory().CreateTranspiler(new Dictionary<string, List<object>>()
        {
            { "is_joe", new List<object> { "=", new List<object> { "field", 2 }, "joe" } },
            { "is_adult", new List<object> { ">", new List<object> { "field", 4 }, 18 } },
            {
                "is_adult_joe", new List<object>
                {
                    "and",
                    new List<object> { "macro", "is_joe" },
                    new List<object> { "macro", "is_adult" }
                }
            }
        });

        var result = transpiler.GenerateSql("postgres",
            new Fields { { 1, "id" }, { 2, "name" }, { 3, "date_joined" }, { 4, "age" } },
            new Query
            {
                {
                    "where", new List<object>
                    {
                        "and",
                        new List<object> { "<", new List<object> { "field", 1 }, 5 },
                        new List<object> { "macro", "is_adult_joe" },
                    }
                }
            });

        Assert.That(result, Is.EqualTo($"""
                                        SELECT *
                                        FROM data
                                        WHERE ("id" < 5) AND (("name" = 'joe') AND ("age" > 18))
                                        ;
                                        """));
    }

    [Test]
    public void CircularMacros_ShouldFail()
    {
        Assert.Throws<InvalidOperationException>(() => new TranspilerFactory().CreateTranspiler(new Dictionary<string, List<object>>()
        {
            {
                "is_good", new List<object>
                {
                    "and",
                    new List<object> { "macro", "is_decent" },
                    new List<object> { ">", new List<object> { "field", 4 }, 18 }
                }
            },
            {
                "is_decent", new List<object>
                {
                    "and",
                    new List<object> { "macro", "is_good" },
                    new List<object> { "<", new List<object> { "field", 5 }, 5 }
                }
            },
        }));
    }
}