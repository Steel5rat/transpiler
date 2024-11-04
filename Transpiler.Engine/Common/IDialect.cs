using Transpiler.Engine.Models;

namespace Transpiler.Engine.Common;

public interface IDialect
{
    bool IsNameMatch(string name);
    
    string GenerateSelect(Fields fields, Query query);
}