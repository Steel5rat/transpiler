using Transpiler.Engine.Models;

namespace Transpiler;

public interface ITranspiler
{
    string GenerateSql(string dialectName, Fields fields, Query query);
}