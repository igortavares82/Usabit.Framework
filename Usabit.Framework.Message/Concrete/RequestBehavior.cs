using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Usabit.Framework.Message.Concrete
{
    public class RequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
                                                                                               where TResponse : IActionResult
    {
        private IEnumerable<IValidator> Validators { get; }

        public RequestBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            Validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var failures = Validators.Select(v => v.Validate(new ValidationContext<TRequest>(request)))
                                     .SelectMany(result => result.Errors)
                                     .Where(f => f != null)
                                     .ToList();

            return (failures.Any() ? await ErrorsAsync(failures) : await next());
        }

        private async Task<TResponse> ErrorsAsync(IEnumerable<ValidationFailure> failures)
        {
            TResponse response = System.Activator.CreateInstance<TResponse>();

            try
            {
                PropertyInfo message = typeof(TResponse).GetProperty("Message");
                message?.SetValue(response, "Invalid request, see validation field for more details.");

                PropertyInfo validations = typeof(TResponse).GetProperty("Validations");
                validations?.SetValue(response, failures.Select(it => it.ErrorMessage).ToList());
            }
            catch { }

            return response;
        }
    }
}
