using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProdutorRuralCadastro.Domain.Interfaces;
using ProdutorRuralCadastro.Infrastructure.DataBase.Repository;
using ProdutorRuralCadastro.Infrastructure.Messaging;
using ProdutorRuralCadastro.Infrastructure.Messaging.Consumers;
using ProdutorRuralCadastro.Infrastructure.Resilience;

namespace ProdutorRuralCadastro.Infrastructure
{
    public static class InfraBootstrapper
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            // Repositories
            services.AddScoped<ICulturaRepository, CulturaRepository>();
            services.AddScoped<IPropriedadeRepository, PropriedadeRepository>();
            services.AddScoped<ITalhaoRepository, TalhaoRepository>();

            // Resilience Policies (Polly)
            services.AddResiliencePolicies();

            // RabbitMQ (apenas se configurado)
            var rabbitEnabled = configuration.GetValue<bool>("RabbitMQ:Enabled");
            if (rabbitEnabled)
            {
                services.AddSingleton<RabbitMqSetup>();
                services.AddHostedService<AlertCreatedConsumer>();
            }
        }
    }
}
