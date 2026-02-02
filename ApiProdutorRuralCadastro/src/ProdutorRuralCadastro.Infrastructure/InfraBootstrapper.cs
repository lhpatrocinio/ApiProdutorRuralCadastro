using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProdutorRuralCadastro.Application.Repository;
using ProdutorRuralCadastro.Infrastructure.DataBase.Repository;

namespace ProdutorRuralCadastro.Infrastructure
{
    public static class InfraBootstrapper
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProdutorRuralCadastroRepository, ProdutorRuralCadastroRepository>();         
        }
    }
}
