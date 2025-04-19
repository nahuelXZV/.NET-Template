using Domain.DTOs.Segurity;
using FluentValidation;

namespace WebClient.Common.Validators.Segurity;

public class CreatePerfilDTOValidator : AbstractValidator<PerfilDTO>
{
    public CreatePerfilDTOValidator()
    {
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(100).WithMessage("{PropertyName} no debe exceder los 100 caracteres.");

        RuleFor(p => p.Descripcion)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(250).WithMessage("{PropertyName} no debe exceder los 250 caracteres.");
    }
}