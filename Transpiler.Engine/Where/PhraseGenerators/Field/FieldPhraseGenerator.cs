using Transpiler.Engine.Where.SyntaxProviders;

namespace Transpiler.Engine.Where.PhraseGenerators.Field;

public class FieldPhraseGenerator : IPhraseGenerator
{
    private readonly string _fieldName;
    private readonly IFieldSyntaxProvider _syntaxProvider;

    public FieldPhraseGenerator(string fieldName, IFieldSyntaxProvider syntaxProvider)
    {
        _fieldName = fieldName;
        _syntaxProvider = syntaxProvider;
    }

    public string GetSql()
    {
        return _syntaxProvider.Field(_fieldName);
    }
}