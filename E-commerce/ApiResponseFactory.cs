using Microsoft.AspNetCore.Mvc;
using Shared.Error_Models;

namespace E_commerce
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiVaidationErrorResponse(ActionContext context)
        {
            var Errors = context.ModelState
                    .Where(e => e.Value.Errors.Any())
                    .Select(e => new ValidationError()
                    {
                        Field = e.Key,
                        Errors = e.Value.Errors.Select(e => e.ErrorMessage)
                    });
            var response = new ValidationErrorToReturn()
            {
                ValidationErrors = Errors
            };
            return new BadRequestObjectResult(response);
        }
    }
}
