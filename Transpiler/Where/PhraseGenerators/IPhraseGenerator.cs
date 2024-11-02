using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where.PhraseGenerators;

public interface IPhraseGenerator
{
    string GetSql();
}