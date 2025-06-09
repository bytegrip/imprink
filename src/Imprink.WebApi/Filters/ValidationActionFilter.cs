namespace Imprink.WebApi.Filters;

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationActionFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var parameter in context.ActionDescriptor.Parameters)
        {
            if (!context.ActionArguments.TryGetValue(parameter.Name, out var argument) || argument == null) continue;
            var argumentType = argument.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
                
            var validators = serviceProvider.GetServices(validatorType).Cast<IValidator>();

            var validatorList = validators as IValidator[] ?? validators.ToArray();
            if (validatorList.Length == 0) continue;
            var validationContext = new ValidationContext<object>(argument);
            var validationTasks = validatorList.Select(v => v.ValidateAsync(validationContext));
            var validationResults = await Task.WhenAll(validationTasks);
                    
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count <= 0) continue;
            var errors = failures.Select(e => new {
                e.PropertyName, e.ErrorMessage 
            });
                        
            context.Result = new BadRequestObjectResult(new { Errors = errors });
            return;
        }
        
        await next();
    }
}