using Transpiler.Models;

namespace Transpiler;

public interface ITranspiler
{
    string GenerateSql(string dialectName, Fields fields, Query query);
}