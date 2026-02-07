using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProdutorRuralCadastro.Application.Services;
using ProdutorRuralCadastro.Application.Services.Interfaces;
using ProdutorRuralCadastro.Application.Validators;

namespace ProdutorRuralCadastro.Application
{
    public static class ApplicationBootstrapper
    {
        public static void Register(IServiceCollection services)
        {
            // Services
            services.AddScoped<ICulturaService, CulturaService>();
            services.AddScoped<IPropriedadeService, PropriedadeService>();
            services.AddScoped<ITalhaoService, TalhaoService>();

            // FluentValidation - registra todos os validators do assembly
            services.AddValidatorsFromAssemblyContaining<PropriedadeCreateRequestValidator>();
        }
    }
}
