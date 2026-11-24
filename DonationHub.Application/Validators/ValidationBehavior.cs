using DonationHub.Domain.Common.Errors;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Validators
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = results.SelectMany(rs => rs.Errors).Where(f => f is not null).ToList();
                if (failures.Any())
                {
                    //return ValidationErrors.Failed;

                    throw new ValidationException(failures);

                }
            }



            return await next(cancellationToken);
        }
    }
}
