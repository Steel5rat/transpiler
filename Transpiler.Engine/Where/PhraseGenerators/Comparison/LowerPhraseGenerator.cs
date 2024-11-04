namespace Transpiler.Engine.Where.PhraseGenerators.Comparison;

public class LowerPhraseGenerator : IPhraseGenerator
{
    private readonly IPhraseGenerator _operand1;
    private readonly IPhraseGenerator _operand2;

    public LowerPhraseGenerator(IPhraseGenerator operand1, IPhraseGenerator operand2)
    {
        _operand1 = operand1;
        _operand2 = operand2;
    }
    
    public string GetSql() => $"{_operand1.GetSql()} < {_operand2.GetSql()}";
}