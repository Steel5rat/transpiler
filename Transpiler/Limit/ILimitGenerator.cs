using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Limit;

public interface ILimitGenerator
{
    string GenerateClause(Query query, Dialect dialect);
}