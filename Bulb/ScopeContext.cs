namespace Bulb;

public record ScopeContext(
    int NumberOfVariablesDeclaredBefore,
    int NumberOfFunctionsDeclaredBefore,
    bool IsStoppable,
    string? ReturnType);