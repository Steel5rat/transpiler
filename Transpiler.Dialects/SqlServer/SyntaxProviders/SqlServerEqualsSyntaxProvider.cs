﻿using Transpiler.Engine.Common;
using Transpiler.Engine.Where.SyntaxProviders;

namespace Transpiler.Dialects.SqlServer.SyntaxProviders;

public class SqlServerEqualsSyntaxProvider : IEqualsSyntaxProvider
{
    public Dialect Dialect => Dialect.SqlServer;
    
    public string AreEqual(string operand1, string operand2)
    {
        return $"{operand1} = {operand2}";
    }

    public string AreNotEqual(string operand1, string operand2)
    {
        return $"{operand1} <> {operand2}";
    }

    public string AreEqual(string operand1, IEnumerable<string> operand2)
    {
        return $"{operand1} IN ({string.Join(", ", operand2)})";
    }

    public string AreNotEqual(string operand1, IEnumerable<string> operand2)
    {
        return $"{operand1} NOT IN ({string.Join(", ", operand2)})";
    }

    public string IsNull(string operand)
    {
        return $"{operand} IS NULL";
    }

    public string IsNotNull(string operand)
    {
        return $"{operand} IS NOT NULL";
    }
}