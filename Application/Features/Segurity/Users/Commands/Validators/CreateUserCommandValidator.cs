using FluentValidation;

namespace Application.Features.Segurity.Users.Commands.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(p => p.RequestRegisterDTO.Username)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(100).WithMessage("{PropertyName} no debe exceder los 100 caracteres.");

        RuleFor(p => p.RequestRegisterDTO.Nombre)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(250).WithMessage("{PropertyName} no debe exceder los 250 caracteres.");

        RuleFor(p => p.RequestRegisterDTO.Apellido)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .MaximumLength(250).WithMessage("{PropertyName} no debe exceder los 250 caracteres.");

        RuleFor(p => p.RequestRegisterDTO.Email)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull()
            .EmailAddress().WithMessage("{PropertyName} no es un email válido.")
            .MaximumLength(100).WithMessage("{PropertyName} no debe exceder los 100 caracteres.");

        RuleFor(p => p.RequestRegisterDTO.Password)
            .NotEmpty().WithMessage("{PropertyName} es requerido.")
            .NotNull();

        RuleFor(p => p.RequestRegisterDTO.PerfilId)
             .NotEmpty().WithMessage("{PropertyName} es requerido.")
             .NotNull();
    }
}

