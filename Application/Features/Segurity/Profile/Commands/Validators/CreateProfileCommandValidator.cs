using FluentValidation;

namespace Application.Features.Segurity.Profile.Commands.Validators;

public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator()
    {
        RuleFor(p => p.PerfilDTO.Nombre)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(100).WithMessage("{PropertyName} no debe exceder los 100 caracteres.");

        RuleFor(p => p.PerfilDTO.Descripcion)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(250).WithMessage("{PropertyName} no debe exceder los 250 caracteres.");

        RuleFor(p => p.PerfilDTO.ListaAccesos)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull();

    }
}