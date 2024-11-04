using System.Collections.Immutable;
using Transpiler.Engine.Common;
using Transpiler.Engine.Models;

namespace Transpiler.Engine.Where.PhraseGenerators;

public interface IPhraseGeneratorFactory
{
    (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand);
    
    IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands, Fields fields, Dialect dialect);
}