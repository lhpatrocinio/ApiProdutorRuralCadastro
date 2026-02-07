using FluentValidation;
using ProdutorRuralCadastro.Application.DTOs.Propriedade;

namespace ProdutorRuralCadastro.Application.Validators;

/// <summary>
/// Validador FluentValidation para atualização de propriedade
/// </summary>
public class PropriedadeUpdateRequestValidator : AbstractValidator<PropriedadeUpdateRequest>
{
    public PropriedadeUpdateRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MinimumLength(2)
            .WithMessage("Nome deve ter no mínimo 2 caracteres")
            .MaximumLength(200)
            .WithMessage("Nome deve ter no máximo 200 caracteres");

        RuleFor(x => x.Estado)
            .MaximumLength(2)
            .WithMessage("Estado deve ter no máximo 2 caracteres (UF)")
            .When(x => !string.IsNullOrEmpty(x.Estado));

        RuleFor(x => x.CEP)
            .Matches(@"^\d{5}-?\d{3}$")
            .WithMessage("CEP inválido (formato: 00000-000)")
            .When(x => !string.IsNullOrEmpty(x.CEP));

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude deve estar entre -90 e 90")
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude deve estar entre -180 e 180")
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x.AreaTotalHa)
            .GreaterThan(0)
            .WithMessage("Área total deve ser maior que zero")
            .When(x => x.AreaTotalHa.HasValue);
    }
}
