using Transpiler.Engine.Common;
using Transpiler.Engine.Models;

namespace Transpiler.Engine.Where;

public interface IWhereGenerator
{
    string GenerateClause(Fields fields, Query query, Dialect dialect);
}