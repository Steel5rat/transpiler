using Transpiler.Engine.Common;
using Transpiler.Engine.Models;

namespace Transpiler.Engine.Limit;

public interface ILimitGenerator
{
    string GenerateClause(Query query, Dialect dialect);
}