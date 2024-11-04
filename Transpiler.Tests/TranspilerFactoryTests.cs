namespace Transpiler.Tests;

[TestFixture]
public class TranspilerFactoryTests
{
    [Test]
    public void IntegrityCheck()
    {
        var factory = new TranspilerFactory();
        
        Assert.That(factory.CreateTranspiler(), Is.Not.Null);
    }
    
}