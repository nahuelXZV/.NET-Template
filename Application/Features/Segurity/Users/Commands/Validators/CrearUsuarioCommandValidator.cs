using Application.Features.Segurity.Users.Command;
using FluentValidation;

namespace Application.Features.Segurity.Users.Commands.Validators;

public class CrearUsuarioCommandValidator : AbstractValidator<CrearUsuarioCommand>
{
    public CrearUsuarioCommandValidator()
    {
        RuleFor(p => p.RequestRegisterDTO.Nombre)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} no debe exceder los 80 caracteres.");

        RuleFor(p => p.RequestRegisterDTO.Apellido)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} no debe exceder los 80 caracteres.");

        RuleFor(p => p.RequestRegisterDTO.Email)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .EmailAddress().WithMessage("{PropertyName} no es un email válido.")
            .MaximumLength(50).WithMessage("{PropertyName} no debe exceder los 80 caracteres.");

        RuleFor(p => p.RequestRegisterDTO.Rol)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} no debe exceder los 20 caracteres.");

        RuleFor(p => p.RequestRegisterDTO.Password)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull();
    }
}

