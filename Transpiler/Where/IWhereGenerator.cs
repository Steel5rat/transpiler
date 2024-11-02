using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where;

public interface IWhereGenerator
{
    string GenerateClause(Fields fields, Query query, Dialect dialect);
}