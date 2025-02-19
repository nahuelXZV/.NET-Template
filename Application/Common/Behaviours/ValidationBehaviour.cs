﻿using Application.Common.Extensions;
using FluentValidation.Results;
using Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        //_logger.LogInformation("----- Validating command {CommandType}", typeName);

        var validationContext = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(validationContext, cancellationToken)));

        List<ValidationFailure> failures = validationResults
            .Where(vr => vr.Errors.Any())
            .SelectMany(result => result.Errors)
            .ToList();

        if (failures.Any())
        {
            //_logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);
            throw new ValidationException($"Errores de validacion para el comando: {typeName}", failures);
        }

        return await next();
    }
}


