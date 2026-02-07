using FluentValidation;
using ProdutorRuralCadastro.Application.DTOs.Talhao;

namespace ProdutorRuralCadastro.Application.Validators;

/// <summary>
/// Validador FluentValidation para criação de talhão
/// </summary>
public class TalhaoCreateRequestValidator : AbstractValidator<TalhaoCreateRequest>
{
    public TalhaoCreateRequestValidator()
    {
        RuleFor(x => x.PropriedadeId)
            .NotEmpty()
            .WithMessage("PropriedadeId é obrigatório");

        RuleFor(x => x.CulturaId)
            .NotEmpty()
            .WithMessage("CulturaId é obrigatório");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MinimumLength(2)
            .WithMessage("Nome deve ter no mínimo 2 caracteres")
            .MaximumLength(100)
            .WithMessage("Nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Codigo)
            .MaximumLength(20)
            .WithMessage("Código deve ter no máximo 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Codigo));

        RuleFor(x => x.AreaHa)
            .GreaterThan(0)
            .WithMessage("Área deve ser maior que zero")
            .LessThanOrEqualTo(99999.99m)
            .WithMessage("Área deve ser menor que 100.000 hectares")
            .When(x => x.AreaHa.HasValue);

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude deve estar entre -90 e 90")
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude deve estar entre -180 e 180")
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.DataPlantio)
            .LessThanOrEqualTo(DateTime.Now.AddDays(365))
            .WithMessage("Data de plantio não pode ser mais de 1 ano no futuro")
            .When(x => x.DataPlantio.HasValue);

        RuleFor(x => x.PrevisaoColheita)
            .GreaterThan(x => x.DataPlantio)
            .WithMessage("Previsão de colheita deve ser posterior à data de plantio")
            .When(x => x.DataPlantio.HasValue && x.PrevisaoColheita.HasValue);
    }
}
