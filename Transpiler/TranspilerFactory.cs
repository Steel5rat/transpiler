using Microsoft.Extensions.DependencyInjection;

namespace Transpiler;

public class TranspilerFactory
{
    public ITranspiler CreateTranspiler()
    {
        return DependenciesContainer.GetContainer(new Dictionary<string, List<object>>()).Services
            .GetRequiredService<ITranspiler>();
    }

    public ITranspiler CreateTranspiler(Dictionary<string, List<object>> macros)
    {
        return DependenciesContainer.GetContainer(macros).Services.GetRequiredService<ITranspiler>();
    }
}