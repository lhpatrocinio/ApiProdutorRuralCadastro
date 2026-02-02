using Microsoft.Extensions.DependencyInjection;
using ProdutorRuralCadastro.Application.Services;
using ProdutorRuralCadastro.Application.Services.Interfaces;

namespace ProdutorRuralCadastro.Application
{
    public static class ApplicationBootstrapper
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IProdutorRuralCadastroService, ProdutorRuralCadastroService>();
        }
    }
}
