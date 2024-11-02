using Transpiler.Models;

namespace Transpiler.Common;

public interface IDialect
{
    bool IsNameMatch(string name);
    
    string GenerateSelect(Fields fields, Query query);
}