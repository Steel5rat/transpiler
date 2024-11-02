using System.Collections.Immutable;
using Transpiler.Common;
using Transpiler.Models;

namespace Transpiler.Where.PhraseGenerators;

public interface IPhraseGeneratorFactory
{
    (bool isMatch, ImmutableList<object?> operandsToBeConverted) IsMatch(object? operand);
    
    IPhraseGenerator CreateGenerator(object? originalOperand, ImmutableList<IPhraseGenerator> operands, Fields fields, Dialect dialect);
}